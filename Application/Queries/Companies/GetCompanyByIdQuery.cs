using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.ConstructionCompanies
{
    public record GetCompanyByIdQuery(string Id) : IRequest<CompanyToReturnDto>;
}
