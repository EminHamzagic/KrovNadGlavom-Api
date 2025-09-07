namespace krov_nad_glavom_api.Data.DTO.AgencyRequest
{
    public class AgencyRequestToReturnDto
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string BuildingId { get; set; }
        public decimal CommissionPercentage { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
        public Domain.Entities.Agency Agency { get; set; }
        public Domain.Entities.ConstructionCompany Company { get; set; }
        public Domain.Entities.Building Building { get; set; }
    }
}