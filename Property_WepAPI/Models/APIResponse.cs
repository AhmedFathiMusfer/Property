using System.Net;

namespace Property_WepAPI.Models
{
    public class APIResponse
    {

       public HttpStatusCode StatusCode { get; set; }
       public  bool IsSuccess { get; set; } = true;
       public  List<string> EroorMessage { get; set; }
       public object Result { get; set; }

    }
}
