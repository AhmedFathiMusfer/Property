using Property_Utility;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;

namespace Property_Wep.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.RefreshToken );
        }

        public TokenDTO? GetToken()
        {
            try
            {
                bool HasAccessToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                bool HasRefreshToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.RefreshToken , out string RefreshToken);
                TokenDTO tokenDTO = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = RefreshToken
                };
                
                return HasAccessToken?tokenDTO:null;    
            }catch
            {
                return null;
            }
          
        }

        public void SetToken(TokenDTO tokenDTO)
        {
            var CookiesOption=new CookieOptions() { Expires=DateTime.UtcNow.AddDays(60) };   
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken, tokenDTO.AccessToken, CookiesOption);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.RefreshToken , tokenDTO.RefreshToken , CookiesOption);
        }
    }
}
