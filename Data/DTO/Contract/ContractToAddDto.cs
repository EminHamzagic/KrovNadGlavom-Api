namespace krov_nad_glavom_api.Data.DTO.Contract
{
    public class ContractToAddDto
    {
        public string UserId { get; set; }
        public string AgencyId { get; set; }
        public string ApartmentId { get; set; }
        public decimal Price { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int InstallmentCount { get; set; }
        public string Status { get; set; } = "Unpaid";
    }
}