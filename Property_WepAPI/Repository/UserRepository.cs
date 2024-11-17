using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Property_WepAPI.Data;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using Property_WepAPI.Repository.IRpository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Property_WepAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string SecertKey;
        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration,IMapper mapper)
        {
            _db = db;
            SecertKey = configuration.GetValue<string>("ApiSettings:Secert");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            LoginResponseDTO loginResponseDTO = new()
            {
                User = null,
                Token = ""
            };
            var user = await _db.ApplicationUsers.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() );
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null||checkPassword==false)
            {
                return loginResponseDTO;
            }
            var role = await _userManager.GetRolesAsync(user);
            //Generate Token
            var tokenhandeler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecertKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var userDTO = _mapper.Map<UserDTO>(user);
            
            var token = tokenhandeler.CreateToken(tokenDescriptor);

            loginResponseDTO.Token = tokenhandeler.WriteToken(token);
            loginResponseDTO.User = userDTO;
           
           

            return loginResponseDTO;
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
                   await _userManager.AddToRoleAsync(user, "admin");
                    var UserToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == regitsterationRequestDTO.UserName);
                    var UserDTO = _mapper.Map<UserDTO>(UserToReturn);
                    return UserDTO;
                }
            }catch(Exception e)
            {
                
            }
            return new UserDTO();
        }
    }
}
