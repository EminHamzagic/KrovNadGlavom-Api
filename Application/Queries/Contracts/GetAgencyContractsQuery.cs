using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public record GetAgencyContractsQuery(string agencyId) : IRequest<List<ContractToReturnDto>>;
}