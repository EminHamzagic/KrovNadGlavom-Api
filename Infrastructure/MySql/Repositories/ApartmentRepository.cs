using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
    public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
		private readonly krovNadGlavomDbContext _context;

		public ApartmentRepository(krovNadGlavomDbContext context) : base(context)
        {
			_context = context;
		}
    }
}