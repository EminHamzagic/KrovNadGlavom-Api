using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.ConstructionCompanies
{
    public class SetCompanyImageCommand : IRequest<string>
    {
		public InstallmentProofToSendDto Dto { get; }
        public SetCompanyImageCommand(InstallmentProofToSendDto dto)
        {
			Dto = dto;
		}
	}
}