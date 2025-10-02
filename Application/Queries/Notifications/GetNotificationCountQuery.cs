using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Notifications
{
    public record GetNotificationCountQuery(string userId) : IRequest<int>;
}