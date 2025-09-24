namespace krov_nad_glavom_api.Data.DTO.AgencyRequest
{
    public class AgencyRequestToAddDto
    {
        public string AgencyId { get; set; }
        public string BuildingId { get; set; }
        public decimal CommissionPercentage { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}