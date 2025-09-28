using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IInstallmentRepository : IRepository<Installment>
    {
        Task<List<Installment>> GetInstallmentsByContractId(string contractId);
        Task<int> GetConfirmedInstallmentsCount(string contractId);
        Task<int> GetNextSequenceNumber(string contractId);
        Task<decimal> GetTotalPaidAmountAsync(string contractId);
    }
}