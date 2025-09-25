using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public class GetAvailableApartmentsQueryHandler : IRequestHandler<GetAvailableApartmentsQuery, PaginatedResponse<ApartmentToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetAvailableApartmentsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<ApartmentToReturnDto>> Handle(GetAvailableApartmentsQuery request, CancellationToken cancellationToken)
        {
            var apartmentsQuery = await _unitofWork.Apartments.GetAllAvailableApartmentsWithBuildings();

            apartmentsQuery = apartmentsQuery.Filter(request.parameters).Sort(request.parameters);

            var totalCount = apartmentsQuery.Count();
            request.parameters.checkOverflow(totalCount);

            var apartmentsPage = await apartmentsQuery
                .Skip((request.parameters.PageNumber - 1) * request.parameters.PageSize)
                .Take(request.parameters.PageSize)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var apartmentsToReturn = apartmentsPage
                .Select(x =>
                {
                    var dto = _mapper.Map<ApartmentToReturnDto>(x.Apartment);
                    dto.Building = _mapper.Map<BuildingToReturnDto>(x.Building);
                    return dto;
                })
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / request.parameters.PageSize);

            return new PaginatedResponse<ApartmentToReturnDto>(
                apartmentsToReturn,
                totalCount,
                request.parameters.PageNumber,
                request.parameters.PageSize,
                totalPages
            );
        }
    }
}