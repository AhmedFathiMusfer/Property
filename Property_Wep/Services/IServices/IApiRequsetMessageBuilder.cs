using Property_Wep.Models;

namespace Property_Wep.Services.IServices
{
    public interface IApiRequsetMessageBuilder
    {

        HttpRequestMessage Build(APIRequest apiRequest);

    }
}
