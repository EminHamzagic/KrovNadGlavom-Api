namespace krov_nad_glavom_api.Domain.Entities
{
    public class Installment
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public int SequenceNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentProof { get; set; }
        public bool IsConfirmed { get; set; }
    }
}