namespace Property_WepAPI.Models
{
    public class ForgetPasswored
    {
        public int Id { get; set; }
        public int code { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime ExpieresAt { get; set; }
        public bool Is_Valid { get; set; }
    }
}
