using Microsoft.AspNetCore.Mvc;
using static Property_Utility.SD;

namespace Property_Wep.Models
{
    public class APIRequest
    {
        public ApiType APIType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }

    }
}
