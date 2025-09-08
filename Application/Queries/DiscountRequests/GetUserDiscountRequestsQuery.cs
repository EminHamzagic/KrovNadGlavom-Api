using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.DiscountRequests
{
    public record GetUserDiscountRequestsQuery(string userId) : IRequest<List<DiscountRequestToReturnDto>>;
}