using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.DiscountRequests
{
    public class GetUserDiscountRequestsQueryHandler : IRequestHandler<GetUserDiscountRequestsQuery, List<DiscountRequestToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserDiscountRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<DiscountRequestToReturnDto>> Handle(GetUserDiscountRequestsQuery request, CancellationToken cancellationToken)
        {
            var discountRequests = await _unitOfWork.DiscountRequests.GetDiscountRequestsByUserId(request.userId, request.status);
            var user = await _unitOfWork.Users.GetByIdAsync(request.userId);

            var aparmentsIds = discountRequests.Select(d => d.ApartmentId).Distinct().ToList();
            var companyIds = discountRequests.Select(d => d.ConstructionCompanyId).Distinct().ToList();
            var agencyIds = discountRequests.Select(d => d.AgencyId).Distinct().ToList();

            var apartments = await _unitOfWork.Apartments.GetApartmentsByIds(aparmentsIds);
            var companies = await _unitOfWork.ConstructionCompanies.GetCompaniesByIds(companyIds);
            var agencies = await _unitOfWork.Agencies.GetAgenciesByIds(agencyIds);

            var apartmentDict = apartments.ToDictionary(a => a.Id);
            var companyDict = companies.ToDictionary(c => c.Id);
            var agencyDict = agencies.ToDictionary(u => u.Id);

            var requestsToReturn = _mapper.Map<List<DiscountRequestToReturnDto>>(discountRequests);
            foreach (var item in requestsToReturn)
            {
                if (apartmentDict.TryGetValue(item.ApartmentId, out var apartment))
                    item.Apartment = apartment;
                if (item.ConstructionCompanyId != null)
                {
                    if (companyDict.TryGetValue(item.ConstructionCompanyId, out var company))
                        item.ConstructionCompany = company;
                }
                if (agencyDict.TryGetValue(item.AgencyId, out var agency))
                    item.Agency = agency;
                
                item.User = user;
            }

            return requestsToReturn;
        }
    }
}