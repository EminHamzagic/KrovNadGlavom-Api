using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Reservations
{
    public record GetReservationsQuery(string userId) : IRequest<Reservation>;
}