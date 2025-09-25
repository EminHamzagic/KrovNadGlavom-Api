using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.DiscountRequests
{
    public class GetCompanyDiscountRequestsQueryHandler : IRequestHandler<GetCompanyDiscountRequestsQuery, List<DiscountRequestToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetCompanyDiscountRequestsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        
        public async Task<List<DiscountRequestToReturnDto>> Handle(GetCompanyDiscountRequestsQuery request, CancellationToken cancellationToken)
        {
            var discountRequests = await _unitofWork.DiscountRequests.GetDiscountRequestsByCompanyId(request.companyId, request.status);
            var company = await _unitofWork.ConstructionCompanies.GetByIdAsync(request.companyId);

            var aparmentsIds = discountRequests.Select(d => d.ApartmentId).Distinct().ToList();
            var userIds = discountRequests.Select(d => d.UserId).Distinct().ToList();
            var agencyIds = discountRequests.Select(d => d.AgencyId).Distinct().ToList();

            var apartments = await _unitofWork.Apartments.GetApartmentsByIds(aparmentsIds);
            var users = await _unitofWork.Users.GetUsersByIds(userIds);
            var agencies = await _unitofWork.Agencies.GetAgenciesByIds(agencyIds);

            var apartmentDict = apartments.ToDictionary(a => a.Id);
            var userDict = users.ToDictionary(u => u.Id);
            var agencyDict = agencies.ToDictionary(u => u.Id);

            var requestsToReturn = _mapper.Map<List<DiscountRequestToReturnDto>>(discountRequests);
            foreach (var item in requestsToReturn)
            {
                if (apartmentDict.TryGetValue(item.ApartmentId, out var apartment))
                    item.Apartment = apartment;
                if (userDict.TryGetValue(item.UserId, out var user))
                    item.User = user;
                if (agencyDict.TryGetValue(item.AgencyId, out var agency))
                    item.Agency = agency;

                item.ConstructionCompany = company;
            }

            return requestsToReturn;
        }
    }
}