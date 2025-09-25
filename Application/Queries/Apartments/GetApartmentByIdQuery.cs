using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public record GetApartmentByIdQuery(string id, string userId) : IRequest<ApartmentToReturnDto>;
}