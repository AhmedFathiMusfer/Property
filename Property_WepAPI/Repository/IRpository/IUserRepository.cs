using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using System.Runtime.InteropServices;

namespace Property_WepAPI.Repository.IRpository
{
    public interface IUserRepository
    {
         bool IsUniqueUser(string username);
          Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO regitsterationRequestDTO);

    }
}
