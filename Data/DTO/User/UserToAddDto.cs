namespace krov_nad_glavom_api.Data.DTO.User
{
    public class UserToAddDto
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}