using AutoMapper.Internal;
using Newtonsoft.Json;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Services.IServices;
using System.Text;

namespace Property_Wep.Services
{
    public class ApiRequsetMessageBuilder : IApiRequsetMessageBuilder
    {
        public HttpRequestMessage Build(APIRequest apiRequest)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept", "*/*");
            }
            else
            {
                message.Headers.Add("Accept", "application/json");
            }

            message.RequestUri = new Uri(apiRequest.Url);

            if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();
                foreach (var prop in apiRequest.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(apiRequest.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }

                    }
                    else
                    {
                        content.Add(new StringContent(value == null ? " " : value.ToString()), prop.Name);
                    }
                }
                message.Content = content;
            }
            else
            {
                if (apiRequest.Data != null)
                {

                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
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
            return message;
        }
    }
}
