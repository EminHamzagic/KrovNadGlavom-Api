using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class CreateMultipleApartmentsCommandHandler : IRequestHandler<CreateMultipleApartmentsCommand, bool>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateMultipleApartmentsCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }
        
        public async Task<bool> Handle(CreateMultipleApartmentsCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetByIdAsync(request.MultipleApartmentsToAddDto.Apartments.First().BuildingId);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            foreach (var item in request.MultipleApartmentsToAddDto.Apartments)
            {
                if (building.FloorCount < item.Floor)
                throw new Exception($"Zgrada ne sadrži traženi sprat. Zgrada ima {building.FloorCount} spratova");


                var ableToAdd = await _unitofWork.Buildings.CanAddApartment(item);
                if (!ableToAdd)
                    throw new Exception("Nije moguće dodati ovaj stan na izabranom spratu");    
            }

            var apartments = _mapper.Map<List<Apartment>>(request.MultipleApartmentsToAddDto.Apartments);

            await _unitofWork.Apartments.AddRangeAsync(apartments);
            await _unitofWork.Save();

            return true;
        }
    }
}