using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class ConfirmInstallmentCommand : IRequest<Installment>
    {
		public string Id { get; }
        public ConfirmInstallmentCommand(string id)
        {
			Id = id;
		}
	}
}