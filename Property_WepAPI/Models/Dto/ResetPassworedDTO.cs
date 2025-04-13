using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models.Dto
{
    public class ResetPassworedDTO
    {
        [EmailAddress ]
        public string Email { get; set; }

        public string Password { get; set; }
        public int  Code { get; set; }
    }
}
