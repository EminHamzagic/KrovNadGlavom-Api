using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.DiscountRequests
{
    public record GetAgencyDiscountRequestsQuery(string agencyId, string status) : IRequest<List<DiscountRequestToReturnDto>>;
}