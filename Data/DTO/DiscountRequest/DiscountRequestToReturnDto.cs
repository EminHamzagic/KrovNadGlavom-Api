namespace krov_nad_glavom_api.Data.DTO.DiscountRequest
{
    public class DiscountRequestToReturnDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AgencyId { get; set; }
        public string ApartmentId { get; set; }
        public string ConstructionCompanyId { get; set; }
        public decimal Percentage { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public Domain.Entities.User User { get; set; }
        public Domain.Entities.Apartment Apartment { get; set; }
        public Domain.Entities.ConstructionCompany ConstructionCompany { get; set; }
        public Domain.Entities.Agency Agency { get; set; }
    }
}