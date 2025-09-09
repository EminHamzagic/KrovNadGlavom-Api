using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
			return await _context.Installments.Where(i => i.ContractId == contractId).ToListAsync();
		}

		public int GetConfirmedInstallmentsCount(string contractId)
		{
			return _context.Installments.Where(i => i.ContractId == contractId && i.IsConfirmed).Count();
		}
    }
}