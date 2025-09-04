using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.ConstructionCompanies
{
    public class CreateConstructionCompanyCommand : IRequest<string>
    {
        public ConstructionCompanyToAddDto ConstructionCompanyToAddDto;
        public CreateConstructionCompanyCommand(ConstructionCompanyToAddDto constructionCompanyToAddDto)
        {
            ConstructionCompanyToAddDto = constructionCompanyToAddDto;
        }
    }
}