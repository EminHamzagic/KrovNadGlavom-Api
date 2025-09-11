namespace krov_nad_glavom_api.Data.DTO.Installment
{
    public class InstallmentProofToSendDto
    {
        public string Id { get; set; }
        public IFormFile File { get; set; }
    }
}