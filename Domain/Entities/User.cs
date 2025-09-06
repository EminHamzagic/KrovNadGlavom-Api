namespace krov_nad_glavom_api.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public string ConstructionCompanyId { get; set; }
        public string AgencyId { get; set; }
    }
}