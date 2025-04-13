using Property_Wep.Models.Dto;
using Property_WepAPI.Models.Dto;
using System.Threading.Tasks;

namespace Property_Wep.Services.IServices
{
    public interface IAuthService
    {

        Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO);

        Task<T> LogoutAsync<T>(TokenDTO tokenDTO);
        Task<T> ForgetPassworedAsync<T>(ForgetPassworedDto forgetPassworedDto);

        Task<T> ForgetPassworedAsyncConfirmationAsync<T>(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO);
        Task<T> ResetPasswored<T>(ResetPassworedDTO resetPassworedDTO);

    }
}
