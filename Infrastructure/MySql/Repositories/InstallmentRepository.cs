using System.Threading.Tasks;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Icao;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class InstallmentRepository : Repository<Installment>, IInstallmentRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public InstallmentRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Installment>> GetInstallmentsByContractId(string contractId)
		{
			return await _context.Installments.Where(i => i.ContractId == contractId).OrderByDescending(i => i.SequenceNumber).ToListAsync();
		}

		public Task<int> GetConfirmedInstallmentsCount(string contractId)
		{
			return Task.FromResult(_context.Installments.Where(i => i.ContractId == contractId && i.IsConfirmed).Count());
		}

		public async Task<int> GetNextSequenceNumber(string contractId)
		{
			return await _context.Installments
				.Where(i => i.ContractId == contractId)
				.OrderByDescending(i => i.CreatedAt)
				.Select(i => i.SequenceNumber)
				.FirstOrDefaultAsync() + 1;
		}

		public async Task<decimal> GetTotalPaidAmountAsync(string contractId)
		{
			return await _context.Installments
				.Where(i => i.ContractId == contractId && i.IsConfirmed)
				.SumAsync(i => (decimal?)i.Amount) ?? 0m;
		}
    }
}