using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models.Dto
{
    public class ForgetPassworedDTO
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
