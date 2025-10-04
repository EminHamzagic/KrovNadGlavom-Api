namespace krov_nad_glavom_api.Domain.Entities
{
    public class Agency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PIB { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string BankAccountNumber { get; set; }
        public bool IsAllowed { get; set; }
    }
}