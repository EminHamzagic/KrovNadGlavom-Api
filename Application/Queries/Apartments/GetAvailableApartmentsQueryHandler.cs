using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public class GetAvailableApartmentsQueryHandler : IRequestHandler<GetAvailableApartmentsQuery, List<ApartmentToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetAvailableApartmentsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<List<ApartmentToReturnDto>> Handle(GetAvailableApartmentsQuery request, CancellationToken cancellationToken)
        {
            var apartments = await _unitofWork.Apartments.GetAllAvailableApartments();

            var buildingIds = apartments.Select(a => a.BuildingId).Distinct().ToList();
            var buildings = await _unitofWork.Buildings.GetBuildingsByIds(buildingIds);
            var buildingDict = buildings.ToDictionary(b => b.Id);

            var apartmentsToReturn = _mapper.Map<List<ApartmentToReturnDto>>(apartments);
            foreach (var item in apartmentsToReturn)
            {
                if (buildingDict.TryGetValue(item.BuildingId, out var building))
                    item.Building = building;
            }

            var apartmentsQuery = apartmentsToReturn.AsQueryable();

            apartmentsQuery = apartmentsQuery.Filter(request.parameters).Sort(request.parameters);

            var totalCount = apartmentsQuery.Count();
			request.parameters.checkOverflow(totalCount);

			var apartmentsPage = apartmentsQuery
				.Skip((request.parameters.PageNumber - 1) * request.parameters.PageSize)
				.Take(request.parameters.PageSize)
				.ToList();

            return apartmentsPage;
        }
    }
}