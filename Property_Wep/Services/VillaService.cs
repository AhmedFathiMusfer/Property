using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;

namespace Property_Wep.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly string villaUrl;
        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaCreateDTO dto,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
              APIType=SD.ApiType.POST,
              Url=villaUrl+ "/api/v1/VillaAPI",
              Data=dto,
              Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaUrl+ "/api/v1/VillaAPI",
                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token )
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaUrl + $"/api/v1/VillaAPI/{id}",
                Token = token

            });
        }

        public Task<T> RemoveAsync<T>(int id,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/v1/VillaAPI/{id}",
                Token = token

            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = villaUrl + $"/api/v1/VillaAPI/{dto.Id}",
                Data=dto,
                Token = token

            });
        }
    }
}
