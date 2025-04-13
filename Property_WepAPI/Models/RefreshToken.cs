using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string JwtTokenId { get; set; }
        public string Refresh_Token { get; set; }
        public bool Is_Valid { get; set; }
        public DateTime ExpieresAt { get; set; }
    }
}
