using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;

namespace Property_Wep.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly string villaNumberUrl;
        public VillaNumberService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            villaNumberUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");

        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
              APIType=SD.ApiType.POST,
              Url= villaNumberUrl + "/api/v1/VillaNumberAPI",
              Data=dto,
              Token=token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaNumberUrl + "/api/v1/VillaNumberAPI",
                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/v1/VillaNumberAPI/{id}",
                Token = token


            });
        }

        public Task<T> RemoveAsync<T>(int id,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = villaNumberUrl + $"/api/v1/VillaNumberAPI/{id}",
                Token = token

            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = villaNumberUrl + $"/api/v1/VillaNumberAPI/{dto.VillaNo}",
                Data=dto,
                Token = token

            });
        }
    }
}
