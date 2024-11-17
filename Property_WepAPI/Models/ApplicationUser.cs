using Microsoft.AspNetCore.Identity;

namespace Property_WepAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
