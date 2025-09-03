using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
    public class AgencyRequestRepository : Repository<AgencyRequest>, IAgencyRequestRepository
    {
		private readonly krovNadGlavomDbContext _context;

		public AgencyRequestRepository(krovNadGlavomDbContext context) : base(context)
        {
			_context = context;
		}
    }
}