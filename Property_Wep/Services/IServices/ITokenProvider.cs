using Property_Wep.Models.Dto;

namespace Property_Wep.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(TokenDTO tokenDTO);
        TokenDTO? GetToken();
        void ClearToken();

    }
}
