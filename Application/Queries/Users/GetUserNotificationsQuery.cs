using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
    public record GetUserNotificationsQuery(string userId) : IRequest<List<Notification>>;
}