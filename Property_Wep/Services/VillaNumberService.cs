using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;

namespace Property_Wep.Services
{
    public class VillaNumberService :  IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly string villaNumberUrl;
        private readonly IBaseService _baseService;
        public VillaNumberService(IHttpClientFactory httpClient,IConfiguration configuration, IBaseService baseService) 
        {
            _httpClient = httpClient;
            villaNumberUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
              APIType=SD.ApiType.POST,
              Url= villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI",
              Data=dto
             
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI"
              

            });
        }

        public async Task<T> GetAsync<T>(int id)  {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/{id}"
          

            });
        }

        public async Task<T> RemoveAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/{id}"

            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/{dto.VillaNo}",
                Data=dto

            });
        }
    }
}
