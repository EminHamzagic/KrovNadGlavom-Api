using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public class GetApartmentByIdQueryHandler : IRequestHandler<GetApartmentByIdQuery, Apartment>
    {
        private readonly IUnitofWork _unitofWork;

        public GetApartmentByIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<Apartment> Handle(GetApartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var apartment = await _unitofWork.Apartments.GetByIdAsync(request.id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            return apartment;
        }
    }
}