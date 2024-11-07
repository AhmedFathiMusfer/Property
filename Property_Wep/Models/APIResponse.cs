using System.Net;

namespace Property_Wep.Models
{
    public class APIResponse
    {

       public HttpStatusCode StatusCode { get; set; }
       public  bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new();
       public object Result { get; set; }

    }
}
