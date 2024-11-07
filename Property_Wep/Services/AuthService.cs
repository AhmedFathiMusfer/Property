using Microsoft.AspNetCore.Authentication;
using Property_Utility;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;
using System.Runtime.InteropServices;

namespace Property_Wep.Services
{
    public class AuthService : BaseService, IAuthService
    {
      //  public APIResponse responseModel { get; set; }
        private IHttpClientFactory httpClient { get; set; }
        private readonly string villaNumberUrl;
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) :base(httpClient)
        {
            this.httpClient = httpClient;
            villaNumberUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");

        }
        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Url = villaNumberUrl+ "/api/v1/AuthUser/login",
                Data =loginRequestDTO
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO)
        {
            return SendAsync<T>(new Models.APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data=registerationRequestDTO,
                Url = villaNumberUrl+ "/api/v1/AuthUser/register"

            });
        }
    }
}
