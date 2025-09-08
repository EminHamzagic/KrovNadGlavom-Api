using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.DiscountRequests
{
    public class GetUserDiscountRequestsQueryHandler : IRequestHandler<GetUserDiscountRequestsQuery, List<DiscountRequestToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetUserDiscountRequestsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<List<DiscountRequestToReturnDto>> Handle(GetUserDiscountRequestsQuery request, CancellationToken cancellationToken)
        {
            var discountRequests = await _unitofWork.DiscountRequests.GetDiscountRequestsByUserId(request.userId);
            var user = await _unitofWork.Users.GetByIdAsync(request.userId);

            var aparmentsIds = discountRequests.Select(d => d.ApartmentId).Distinct().ToList();
            var companyIds = discountRequests.Select(d => d.ConstructionCompanyId).Distinct().ToList();

            var apartments = await _unitofWork.Apartments.GetApartmentsByIds(aparmentsIds);
            var companies = await _unitofWork.ConstructionCompanies.GetCompaniesByIds(companyIds);

            var apartmentDict = apartments.ToDictionary(a => a.Id);
            var companyDict = companies.ToDictionary(c => c.Id);

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
                
                item.User = user;
            }

            return requestsToReturn;
        }
    }
}