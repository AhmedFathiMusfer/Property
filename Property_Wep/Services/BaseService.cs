using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace Property_Wep.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        private IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiRequsetMessageBuilder _apiRequsetMessageBuilder;
        private string villaUrl;
        public BaseService (IHttpClientFactory httpClient, ITokenProvider tokenProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IApiRequsetMessageBuilder apiRequsetMessageBuilder)
        {
            this.httpClient = httpClient;
            responseModel = new();
            _tokenProvider = tokenProvider;
            villaUrl = configuration.GetValue<string>("ServiecUrls:VillaAPI");
            _httpContextAccessor = httpContextAccessor;
            _apiRequsetMessageBuilder = apiRequsetMessageBuilder;   
        }



        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var Client = httpClient.CreateClient("PrppertyAPI");

               

                var MessageFactory = () =>
                {

                  return  _apiRequsetMessageBuilder.Build(apiRequest);
                };
              

                HttpResponseMessage HttpResponseMessage = null;

                HttpResponseMessage = await SendWithRefreshTokenAsync(Client, MessageFactory,withBearer);
                APIResponse FinalApiResponse = new APIResponse()
                {
                    IsSuccess = false
                };
                try
                {
                    switch (HttpResponseMessage.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            FinalApiResponse.ErrorMessages = new List<string>() { "Not Found" };
                            break;
                        case HttpStatusCode.Unauthorized:
                            FinalApiResponse.ErrorMessages = new List<string>() { "Unauthorized" };
                            break;
                        case HttpStatusCode.Forbidden:
                            FinalApiResponse.ErrorMessages = new List<string>() { "Access dineid" };
                            break;
                        case HttpStatusCode.InternalServerError:
                            FinalApiResponse.ErrorMessages = new List<string>() { "Internal Server Error" };
                            break;
                        default:
                            var ApiContent =   await HttpResponseMessage.Content.ReadAsStringAsync();
                            FinalApiResponse.IsSuccess = true;
                            FinalApiResponse =JsonConvert.DeserializeObject<APIResponse>(ApiContent);
                            break;
                    }
                  
                    //if (apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound || apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    //{
                    //    APIResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    //    APIResponse.IsSuccess = false;
                    //    var res = JsonConvert.SerializeObject(APIResponse);
                    //   var ReturnObj= JsonConvert.DeserializeObject<T>(res);
                    //    return ReturnObj;


                    //}
                }
                catch(AuthException)
                {
                    throw;
                }
                catch(Exception e)
                {

                    FinalApiResponse.ErrorMessages=new List<string> { "ErrorEncountred " ,e.Message};
                    
                }
                var ApiRespons = JsonConvert.SerializeObject(FinalApiResponse);
                var ReturnObj = JsonConvert.DeserializeObject<T>(ApiRespons);
                return ReturnObj;
             

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

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient ,Func<HttpRequestMessage> messageFactory,bool withBearer)
        {
           
            if (!withBearer)
            {
                return await httpClient.SendAsync(messageFactory());

            }
            else
            {
                var tockenDto = _tokenProvider.GetToken();
                try
                {

                    if (tockenDto != null && !string.IsNullOrEmpty(tockenDto.AccessToken))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tockenDto.AccessToken);
                    }

                    var response = await httpClient.SendAsync(messageFactory());
                    if (response.IsSuccessStatusCode)
                    {

                        return response;


                    }
                    if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
                    {

                        await InvokeRefreshTokenEndpoint(httpClient, tockenDto.RefreshToken, tockenDto.AccessToken);
                        Console.WriteLine("arrive");
                        var respons = await httpClient.SendAsync(messageFactory());
                        Console.WriteLine("arrive2");

                    }
                    return response;
                }
                catch (AuthException )
                {
                    throw;
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await InvokeRefreshTokenEndpoint(httpClient, tockenDto.RefreshToken, tockenDto.AccessToken);
                        Console.WriteLine("arrive3");
                        return  await httpClient.SendAsync(messageFactory());
                    }
                    throw;
                }
            }
         
          
            
        }
        private async  Task InvokeRefreshTokenEndpoint(HttpClient httpClient,string existingRefreshToken,string existingAccessToken )
        {
            
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri(villaUrl + $"/api/{SD.CurrentVersion}/AuthUser/refresh");
            message.Content = new StringContent(JsonConvert.SerializeObject(new TokenDTO()
            {
                AccessToken =existingAccessToken,
                RefreshToken = existingRefreshToken
            }),Encoding.UTF8,"application/json");
          var response=   await httpClient.SendAsync(message);
            var content = await  response.Content.ReadAsStringAsync();
            var ApiResponse= JsonConvert.DeserializeObject<APIResponse>(content);
            if (ApiResponse.IsSuccess!=true)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
              
            }
            else
            {
                var tokenDtoStr = JsonConvert.SerializeObject(ApiResponse.Result);
                var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(tokenDtoStr);
                if (tokenDto != null || !string.IsNullOrEmpty(tokenDto.AccessToken))
                {
                   await  SignInWithNewToken(tokenDto);
                    httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",tokenDto.AccessToken);
                }
              
            }


        }
        private async  Task SignInWithNewToken(TokenDTO tokenDto)
        {
            var idntity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var JwtToken = JwtTokenHandler.ReadJwtToken(tokenDto.AccessToken);
            idntity.AddClaim(new Claim(ClaimTypes.Role, JwtToken.Claims.FirstOrDefault(c => c.Type == "role").Value));
            idntity.AddClaim(new Claim(ClaimTypes.Name, JwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));
            var Principal = new ClaimsPrincipal();
            Principal.AddIdentity(idntity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
            _tokenProvider.SetToken(tokenDto);
        }
    }

}
