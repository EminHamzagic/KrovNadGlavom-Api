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
            _unitofWork.Installments.Update(installment);
            await _unitofWork.Save();

            var contract = await _unitofWork.Contracts.GetByIdAsync(installment.ContractId);
            var confirmedInstallments = await _unitofWork.Installments.GetConfirmedInstallmentsCount(installment.ContractId);
            if (contract.InstallmentCount == confirmedInstallments)
            {
                contract.Status = "Paid";
            }
            else
            {
                var seqNumber = await _unitofWork.Installments.GetNextSequenceNumber(contract.Id);

                decimal amount = contract.InstallmentAmount;

                if (seqNumber == contract.InstallmentCount)
                {
                    var totalPaid = await _unitofWork.Installments.GetTotalPaidAmountAsync(contract.Id);
                    amount = contract.Price - totalPaid;
                }
                var nextInstallment = new Installment
                {
                    Id = Guid.NewGuid().ToString(),
                    ContractId = contract.Id,
                    SequenceNumber = seqNumber,
                    Amount = amount,
                    DueDate = DateTime.Now.AddDays(30),
                    IsConfirmed = false,
                    CreatedAt = DateTime.Now
                };

                await _unitofWork.Installments.AddAsync(nextInstallment);
            }
            _unitofWork.Contracts.Update(contract);

            await _unitofWork.Save();
            return installment;
        }
    }
}