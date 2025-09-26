namespace krov_nad_glavom_api.Data.DTO.User
{
    public class UserChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
