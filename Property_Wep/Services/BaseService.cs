using Newtonsoft.Json;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Services.IServices;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace Property_Wep.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        private IHttpClientFactory httpClient { get; set; }
        public BaseService (IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
            responseModel = new();
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var Client = httpClient.CreateClient("PrppertyAPI");

                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.APIType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                apiResponse = await Client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse APIResponse= JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound || apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        APIResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        APIResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(APIResponse);
                       var ReturnObj= JsonConvert.DeserializeObject<T>(res);
                        return ReturnObj;


                    }
                }
                catch(Exception e)
                {

                    var ExceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return ExceptionResponse;
                }
                
                var APIResponseObj = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponseObj;

            }catch(Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string>
                    {
                        Convert.ToString(e.Message)
                    },
                    IsSuccess = false

                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
            }
          
    }
}
