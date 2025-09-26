namespace krov_nad_glavom_api.Domain.Entities
{
    public class DiscountRequest
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AgencyId { get; set; }
        public string ApartmentId { get; set; }
        public string ConstructionCompanyId { get; set; }
        public decimal Percentage { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string RejectReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}