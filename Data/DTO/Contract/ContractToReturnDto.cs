using krov_nad_glavom_api.Data.DTO.Apartment;

namespace krov_nad_glavom_api.Data.DTO.Contract
{
    public class ContractToReturnDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AgencyId { get; set; }
        public string ApartmentId { get; set; }
        public decimal Price { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int InstallmentCount { get; set; }
        public string Status { get; set; }
        public int LateCount { get; set; }
        public List<Domain.Entities.Installment> Installments { get; set; }
        public Domain.Entities.User User { get; set; }
        public Domain.Entities.Agency Agency { get; set; }
        public ApartmentToReturnDto Apartment { get; set; }
        
    }
}