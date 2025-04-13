using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models.Dto
{
    public class ForgetPassworedDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
