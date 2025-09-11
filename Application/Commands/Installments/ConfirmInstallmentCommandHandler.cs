using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class ConfirmInstallmentCommandHandler : IRequestHandler<ConfirmInstallmentCommand, Installment>
    {
        private readonly IUnitofWork _unitofWork;
        public ConfirmInstallmentCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<Installment> Handle(ConfirmInstallmentCommand request, CancellationToken cancellationToken)
        {
            var installment = await _unitofWork.Installments.GetByIdAsync(request.Id);
            if (installment == null)
                throw new Exception("Rata nije pronađena");

            installment.IsConfirmed = true;
            installment.PaymentDate = DateTime.Now;
            await _unitofWork.Save();

            var contract = await _unitofWork.Contracts.GetByIdAsync(installment.ContractId);
            var confirmedInstallments = _unitofWork.Installments.GetConfirmedInstallmentsCount(installment.ContractId);
            if (contract.InstallmentCount == confirmedInstallments)
            {
                contract.Status = "Paid";
                await _unitofWork.Save();
            }

            return installment;
        }
    }
}