using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.ConstructionCompanies
{
    public class UpdateConstructionCompanyCommand : IRequest<ConstructionCompany>
    {
		public string Id { get; }
		public ConstructionCompanyToUpdateDto ConstructionCompanyToUpdateDto { get; }
        public UpdateConstructionCompanyCommand(string id, ConstructionCompanyToUpdateDto constructionCompanyToUpdateDto)
        {
			Id = id;
			ConstructionCompanyToUpdateDto = constructionCompanyToUpdateDto;
		}
	}
}