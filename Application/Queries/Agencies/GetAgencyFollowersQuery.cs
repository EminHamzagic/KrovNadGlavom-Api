using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public record GetAgencyFollowersQuery(string Id) : IRequest<List<User>>;
}