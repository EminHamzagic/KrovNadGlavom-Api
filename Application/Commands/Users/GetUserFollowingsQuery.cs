using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public record GetUserFollowingsQuery(string Id) : IRequest<List<Agency>>;
}