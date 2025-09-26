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
            var (apartmentsPage, totalCount, totalPages) = await _unitofWork.Apartments.GetAllAvailableApartmentsWithBuildings(request.parameters);

            var apartmentsToReturn = apartmentsPage
                .Select(x =>
                {
                    var dto = _mapper.Map<ApartmentToReturnDto>(x.Apartment);
                    dto.Building = _mapper.Map<BuildingToReturnDto>(x.Building);
                    return dto;
                })
                .ToList();

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