namespace krov_nad_glavom_api.Data.DTO.Installment
{
    public class InstallmentToAddDto
    {
        public string ContractId { get; set; }
        public int SequenceNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}