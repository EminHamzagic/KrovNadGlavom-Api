using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public record GetUserContractsQuery(string userId);
}