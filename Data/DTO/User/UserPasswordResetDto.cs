namespace krov_nad_glavom_api.Data.DTO.User
{
    public class UserPasswordResetDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}