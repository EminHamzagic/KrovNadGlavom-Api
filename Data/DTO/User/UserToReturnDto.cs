namespace krov_nad_glavom_api.Data.DTO.User
{
    public class UserToReturnDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public string ConstructionCompanyId { get; set; }
        public string AgencyId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Domain.Entities.Reservation Reservation { get; set; }
    }
}