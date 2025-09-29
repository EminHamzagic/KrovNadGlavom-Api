using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateApartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.GetByIdAsync(request.ApartmentToAddDto.BuildingId);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            if (building.FloorCount < request.ApartmentToAddDto.Floor)
                throw new Exception($"Zgrada ne sadrži traženi sprat. Zgrada ima {building.FloorCount} spratova");

            var apartment = _mapper.Map<Apartment>(request.ApartmentToAddDto);

            var ableToAdd = await _unitOfWork.Buildings.CanAddApartment(request.ApartmentToAddDto);
            if (!ableToAdd)
                throw new Exception("Nije moguće dodati ovaj stan na izabranom spratu");

            await _unitOfWork.Apartments.AddAsync(apartment);
            await _unitOfWork.Save();

            return apartment.Id;
        }
    }
}