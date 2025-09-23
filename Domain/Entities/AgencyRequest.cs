namespace krov_nad_glavom_api.Domain.Entities
{
    public class AgencyRequest
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string BuildingId { get; set; }
        public decimal CommissionPercentage { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
        public bool IsDeleted { get; set; }
    }
}