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
        private string SecertKey;
        public UserRepository(ApplicationDbContext db,IConfiguration configuration)
        {
            _db = db;
            SecertKey = configuration.GetValue<string>("ApiSettings:Secert");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
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
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() && u.Password==loginRequestDTO.Password);
            if (user == null)
            {
                return loginResponseDTO;
            }

            //Generate Token
            var tokenhandeler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecertKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            user.Password = "";
            var token = tokenhandeler.CreateToken(tokenDescriptor);

            loginResponseDTO.Token = tokenhandeler.WriteToken(token);
            loginResponseDTO.User = user;
           
           

            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO regitsterationRequestDTO)
        {
            LocalUser user = new LocalUser()
            {
                UserName = regitsterationRequestDTO.UserName,
                Name = regitsterationRequestDTO.Name,
                Password = regitsterationRequestDTO.Password,
                Role = regitsterationRequestDTO.Role

            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
