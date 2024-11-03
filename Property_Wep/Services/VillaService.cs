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

        public Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
              APIType=SD.ApiType.POST,
              Url=villaUrl+ "/api/VillaAPI",
              Data=dto
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaUrl+"/api/VillaAPI",

            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaUrl + $"/api/VillaAPI/{id}",

            });
        }

        public Task<T> RemoveAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/VillaAPI/{id}",

            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = villaUrl + $"/api/VillaAPI/{dto.Id}",
                Data=dto

            });
        }
    }
}
