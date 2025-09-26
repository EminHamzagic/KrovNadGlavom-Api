namespace krov_nad_glavom_api.Data.DTO.DiscountRequest
{
    public class DiscountRequestToAddDto
    {
        public string UserId { get; set; }
        public string AgencyId { get; set; }
        public string ApartmentId { get; set; }
        public string ConstructionCompanyId { get; set; }
        public decimal Percentage { get; set; }
        public string Status { get; set; } = "Pending";
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}