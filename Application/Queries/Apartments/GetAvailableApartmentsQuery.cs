using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public record GetAvailableApartmentsQuery(QueryStringParameters parameters) : IRequest<List<ApartmentToReturnDto>>;
}