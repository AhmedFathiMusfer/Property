using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;

namespace Property_Wep.Services
{
    public class VillaService : IVillaService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly string villaUrl;
        private readonly IBaseService _baseService;
        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration,IBaseService baseService) { 
            _httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
              APIType=SD.ApiType.POST,
              Url=villaUrl+ $"/api/{SD.CurrentVersion}/VillaAPI",
              Data=dto,
              ContentType =SD.ContentType.MultipartFormData 
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaUrl+ $"/api/{SD.CurrentVersion}/VillaAPI"

            });
        }

        public async Task<T> GetAsync<T>(int id )
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/{id}"

            });
        }

        public async Task<T> RemoveAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/{id}"
                

            });
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/{dto.Id}",
                Data=dto,
                ContentType =SD.ContentType.MultipartFormData

            });
        }
    }
}
