using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateApartmentCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetByIdAsync(request.ApartmentToAddDto.BuildingId);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            if (building.FloorCount < request.ApartmentToAddDto.Floor)
                throw new Exception($"Zgrada ne sadrži traženi sprat. Zgrada ima {building.FloorCount} spratova");

            var apartment = _mapper.Map<Apartment>(request.ApartmentToAddDto);

            var ableToAdd = await _unitofWork.Buildings.CanAddApartment(request.ApartmentToAddDto);
            if (!ableToAdd)
                throw new Exception("Nije moguće dodati ovaj stan na izabranom spratu");

            await _unitofWork.Apartments.AddAsync(apartment);
            await _unitofWork.Save();

            return apartment.Id;
        }
    }
}