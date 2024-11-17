namespace Property_WepAPI.Models.Dto
{
    public class LoginResponseDTO
    {
        public UserDTO? User { get; set; }
        public string? Token { get; set; }
    }
}
