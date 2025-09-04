using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public record GetApartmentByIdQuery(string id) : IRequest<Apartment>;
}