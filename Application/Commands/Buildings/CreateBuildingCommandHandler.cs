using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;
using ZstdSharp.Unsafe;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class CreateBuildingCommandHandler : IRequestHandler<CreateBuildingCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateBuildingCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            var existingBuilding = await _unitofWork.Buildings.GetBuildingById(request.BuildingToAddDto.ParcelNumber);
            if (existingBuilding != null)
                throw new Exception("Postoji zgrada na sa unetim brojem parcele");

            var building = _mapper.Map<Building>(request.BuildingToAddDto);
            building.Id = Guid.NewGuid().ToString();

            _unitofWork.Buildings.AddAsync(building);

            var apartmentsToAdd = _mapper.Map<List<ApartmentToAddDto>, List<Apartment>>(request.BuildingToAddDto.Apartments);
            foreach (var apartment in apartmentsToAdd)
            {
                if(apartment.Floor > building.FloorCount)
                    throw new Exception($"Zgrada ne sadrži traženi sprat. Zgrada ima {building.FloorCount} spratova");
                    
                apartment.BuildingId = building.Id;
                apartment.Id = Guid.NewGuid().ToString();
            }
            await _unitofWork.Apartments.AddRangeAsync(apartmentsToAdd);

            var garagesToAdd = _mapper.Map<List<Garage>>(request.BuildingToAddDto.Garages);

            var duplicates = garagesToAdd
                .GroupBy(g => g.SpotNumber)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Any())
            {
                throw new Exception($"Duplikat garažnih mesta pronađen: {string.Join(", ", duplicates)}");
            }
            
            if (garagesToAdd.Count() > building.GarageSpotCount)
                throw new Exception("Broj garažnih mesta je veći od broja garažnih mesta u zgradi");

            foreach (var garage in garagesToAdd)
            {
                garage.Id = Guid.NewGuid().ToString();
                garage.BuildingId = building.Id;
            }
            await _unitofWork.Garages.AddRangeAsync(garagesToAdd);

            var priceList = _mapper.Map<PriceList>(request.BuildingToAddDto.PriceList);
            priceList.Id = Guid.NewGuid().ToString();
            priceList.BuildingId = building.Id;
            _unitofWork.PriceLists.AddAsync(priceList);

            await _unitofWork.Save();

            return building.Id;
        }
    }
}