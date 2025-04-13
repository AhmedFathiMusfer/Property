using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models.Dto
{
    public class ForgetPassworedConfirmationDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
