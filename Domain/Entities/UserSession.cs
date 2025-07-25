namespace krov_nad_glavom_api.Domain.Entities
{
    public class UserSession
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public string Role { get; set; }
    }
}