using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Property_WepAPI.Data;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using Property_WepAPI.Repository.IRpository;
using Property_WepAPI.Services;
using Property_WepAPI.Services.IServices;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Property_WepAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private string SecertKey;
        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration,IMapper mapper
            ,IEmailSender  emailSender)
        {
            _db = db;
            SecertKey = configuration.GetValue<string>("ApiSettings:Secert");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            TokenDTO TokenDTO = new()
            {

                AccessToken = ""
            };

            var user = await _db.ApplicationUsers.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() );
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            
            if (user == null||checkPassword==false)
            {
                return TokenDTO;
            }

            var JWTTokenId = $"JTI{Guid.NewGuid()}";
            var Refreshtoken =await  CreateNewRefreshToken(user.Id, JWTTokenId);
            var JWTToken = await GetAccessTokenAsync(user,JWTTokenId);
            TokenDTO.AccessToken = JWTToken;
            TokenDTO.RefreshToken = Refreshtoken;
            TokenDTO = await RefreshAccessToken(TokenDTO);

           
           
           
           

            return TokenDTO;
        }

        public async Task<UserDTO> Register(RegisterationRequestDTO regitsterationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = regitsterationRequestDTO.UserName,
                Name = regitsterationRequestDTO.Name,
                Email= regitsterationRequestDTO.UserName,
                NormalizedEmail= regitsterationRequestDTO.UserName.ToUpper()

            };
            try
            {
                var result = await _userManager.CreateAsync(user, regitsterationRequestDTO.Password);
                
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await  _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                   await _userManager.AddToRoleAsync(user, regitsterationRequestDTO.Role);
                    var UserToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == regitsterationRequestDTO.UserName);
                    var UserDTO = _mapper.Map<UserDTO>(UserToReturn);
                    return UserDTO;
                }
            }catch(Exception e)
            {
                
            }
            return new UserDTO();
        }
        private  async Task<string> GetAccessTokenAsync(ApplicationUser user,string JWTTokenId)
        {
            var role = await _userManager.GetRolesAsync(user);
            //Generate Token
            var tokenhandeler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecertKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti,JWTTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id)


                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //  var userDTO = _mapper.Map<UserDTO>(user);

            var token = tokenhandeler.CreateToken(tokenDescriptor);

            return tokenhandeler.WriteToken(token);
        }
       
        private bool GetAccessTokenData(string AccessToken, string excectedUserId, string excectedTokenId)
        {
            try
            {
                var tokenHandler=new JwtSecurityTokenHandler();
                var jwt=tokenHandler.ReadJwtToken(AccessToken);
                var userId = jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub).Value;
                var tokenId = jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti).Value;
                if(excectedUserId!=userId||excectedTokenId !=tokenId)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false ;
            }

        }
        public async Task RevokeAccessToken(TokenDTO tokenDTO)
        {
           var existingRefreshToken=await  _db.refreshTokens.FirstOrDefaultAsync(r=>r.Refresh_Token==tokenDTO.RefreshToken);
            if (existingRefreshToken==null)
            {
                return;
            }
            var accessTokenData =  GetAccessTokenData(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!accessTokenData)
            {
                return;
            }
            await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
        }
        public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        {
            var   existingRefreshToken=await _db.refreshTokens.FirstOrDefaultAsync(r=>r.Refresh_Token==tokenDTO.RefreshToken);
            if (existingRefreshToken == null)
            {
               return new TokenDTO();
            }
            if(!existingRefreshToken.Is_Valid)
            {
                await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
                return new TokenDTO();
            }
            var accessTokenData =  GetAccessTokenData(tokenDTO.AccessToken,existingRefreshToken.UserId,existingRefreshToken.JwtTokenId);
             if(!accessTokenData) {

                await MarkTokenAsInvalid(existingRefreshToken);

                return new TokenDTO();
            }
            if (existingRefreshToken.ExpieresAt < DateTime.UtcNow)
            {
              await   MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }

            var newRefreshAccessToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

           await  MarkTokenAsInvalid(existingRefreshToken);

            var user= await _db.ApplicationUsers.FirstOrDefaultAsync(u=>u.Id ==existingRefreshToken.UserId);
            if (user == null)
            {
                return new TokenDTO();
            }
            var newAccessToken = await GetAccessTokenAsync(user, existingRefreshToken.JwtTokenId);

            return new TokenDTO { RefreshToken = newRefreshAccessToken, AccessToken = newAccessToken };
           
            throw new NotImplementedException();
           
        }
        private async Task<string> CreateNewRefreshToken(string userId,string tokenId)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                UserId =userId ,
                JwtTokenId = tokenId ,
                ExpieresAt = DateTime.UtcNow.AddMinutes(3),
                Is_Valid = true,
                Refresh_Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
            };
           await  _db.refreshTokens.AddAsync(refreshToken);
            await   _db.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }
        private async Task MarkAllTokenInChainAsInvalid(string UserId,string tokenId)
        {
            var refreshTokens = _db.refreshTokens.Where(r => r.UserId == UserId && r.JwtTokenId == tokenId).ToList();
            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.Is_Valid = false;
            }
            _db.refreshTokens.UpdateRange(refreshTokens);
           await  _db.SaveChangesAsync();
        }
        private  Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.Is_Valid = false;
            return _db.SaveChangesAsync();
        }

        public async Task<ForgetPassworedDTO> ForgetPasswored(ForgetPassworedDTO forgetPassworedDTO)
        {
            var user=await _db.ApplicationUsers.FirstOrDefaultAsync(u=>u.Email == forgetPassworedDTO.Email);
            if (user == null)
            {
                return null;
            }
            try
            {
                var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var code = GenerateRandomNumber();

                ForgetPasswored forgetPasswored = new ForgetPasswored()
                {
                    code = code,
                    Email = user.Email,
                    Token = Token,
                    ExpieresAt = DateTime.UtcNow.AddMinutes(2),
                    Is_Valid = true
                };
                await _db.fogetPassworeds.AddAsync(forgetPasswored);
                await _db.SaveChangesAsync();
                await _emailSender.SendEmailAsync(user.Email,
                     "Reset Password",
                     $"Verification codes: <h4>{code}</h4>.");
                
                return forgetPassworedDTO;
            }catch (Exception ex)
            {
                return null;
            }
           

        }

        public async Task<ForgetPassworedConfirmationDTO> ForgetPassworedConfirmation(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO)
        {
            var GetToken=  await _db.fogetPassworeds.FirstOrDefaultAsync(t=>t.Email==forgetPassworedConfirmationDTO.Email
            &&t.code==forgetPassworedConfirmationDTO.Code);
            if( GetToken == null)
            {
                return null ;
            }
            if (!GetToken.Is_Valid)
            {
                return null;
            }
            if(GetToken.ExpieresAt<DateTime.UtcNow)
            {
                GetToken.Is_Valid = false;
               await  _db.SaveChangesAsync();
                return null;
            }

            ForgetPassworedConfirmationDTO forgetPassworedDTO = new ForgetPassworedConfirmationDTO()
            {
                Email = forgetPassworedConfirmationDTO.Email,
                Code= forgetPassworedConfirmationDTO.Code,
            };
            return forgetPassworedDTO;
        }
        private int GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(100000, 1000000); 
        }

        public async Task<bool> ResetPasswored(ResetPassworedDTO resetPassworedDTO)
        {
          var user=await  _db.ApplicationUsers.FirstOrDefaultAsync(u=>u.Email ==resetPassworedDTO.Email);
            if( user == null )
            {
                return false;
            }

            var Token = await _db.fogetPassworeds.FirstOrDefaultAsync(t=>t.code==resetPassworedDTO.Code&& t.Is_Valid==true  &&t.Email==resetPassworedDTO.Email);
            if( Token == null ) { 
            
              return false;
            }
           var result= await  _userManager.ResetPasswordAsync(user, Token.Token, resetPassworedDTO.Password);
            if(result.Succeeded )
            {
                Token.Is_Valid = false;
                await  _db.SaveChangesAsync();
                return true;
            }
            return false;


        }
    }
}
