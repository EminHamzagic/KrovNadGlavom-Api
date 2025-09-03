using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
    public class AgencyRepository : Repository<Agency>, IAgencyRepository
    {
		private readonly krovNadGlavomDbContext _context;

		public AgencyRepository(krovNadGlavomDbContext context) : base(context)
        {
			_context = context;
		}
    }
}