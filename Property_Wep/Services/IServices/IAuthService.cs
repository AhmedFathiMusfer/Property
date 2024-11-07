using Property_Wep.Models.Dto;

namespace Property_Wep.Services.IServices
{
    public interface IAuthService
    {

        Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO);

    }
}
