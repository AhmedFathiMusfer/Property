using Microsoft.AspNetCore.Authentication;
using Property_Utility;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;
using Property_WepAPI.Models.Dto;
using System.Runtime.InteropServices;

namespace Property_Wep.Services
{
    public class AuthService :  IAuthService
    {
      //  public APIResponse responseModel { get; set; }
        private IHttpClientFactory httpClient { get; set; }
        private readonly string villaNumberUrl;
        
        private readonly IBaseService _baseService;
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration, ITokenProvider tokenProvider,IBaseService baseService) { 
            this.httpClient = httpClient;
            villaNumberUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");
          
            _baseService = baseService; 
        }
        public async Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return await  _baseService.SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Url = villaNumberUrl+ $"/api/{SD.CurrentVersion}/AuthUser/login",
                Data =loginRequestDTO
            },withBearer:false);
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO)
        {
            return await  _baseService.SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data=registerationRequestDTO,
                Url = villaNumberUrl+ $"/api/{SD.CurrentVersion}/AuthUser/register"

            },withBearer:false);
        }
        public async Task<T> LogoutAsync<T>(TokenDTO tokenDTO)
        {
            return await _baseService.SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data = tokenDTO,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/AuthUser/revoke"

            });
        }
        public async Task<T> ForgetPassworedAsync<T>(ForgetPassworedDto forgetPassworedDto)
        {
            return await _baseService.SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data = forgetPassworedDto,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/AuthUser/forgetPasswored"
                

            },withBearer:false);
        }
        public async Task<T> ForgetPassworedAsyncConfirmationAsync<T>(ForgetPassworedConfirmationDTO  forgetPassworedConfirmationDTO)
        {
            return await _baseService.SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data = forgetPassworedConfirmationDTO,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/AuthUser/forgetPassworedConfirmation"


            }, withBearer: false);
        }
        public async Task<T> ResetPasswored<T>(ResetPassworedDTO resetPassworedDTO)
        {
            return await _baseService.SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data = resetPassworedDTO,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/AuthUser/resetPasswored"


            }, withBearer: false);
        }
    }
}
