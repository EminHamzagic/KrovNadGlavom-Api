using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class ConfirmInstallmentCommandHandler : IRequestHandler<ConfirmInstallmentCommand, Installment>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly INotificationService _notificationService;

		public ConfirmInstallmentCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
			_notificationService = notificationService;
		}

        public async Task<Installment> Handle(ConfirmInstallmentCommand request, CancellationToken cancellationToken)
        {
            var installment = await _unitOfWork.Installments.GetByIdAsync(request.Id);
            if (installment == null)
                throw new Exception("Rata nije pronađena");

            installment.IsConfirmed = true;
            installment.PaymentDate = DateTime.Now;
            _unitOfWork.Installments.Update(installment);
            await _notificationService.SendNotificationsForInstallmentConfirm(installment);
            await _unitOfWork.Save();

            var contract = await _unitOfWork.Contracts.GetByIdAsync(installment.ContractId);
            var confirmedInstallments = await _unitOfWork.Installments.GetConfirmedInstallmentsCount(installment.ContractId);
            if (contract.InstallmentCount == confirmedInstallments)
            {
                contract.Status = "Paid";
            }
            else
            {
                var seqNumber = await _unitOfWork.Installments.GetNextSequenceNumber(contract.Id);

                decimal amount = contract.InstallmentAmount;

                if (seqNumber == contract.InstallmentCount)
                {
                    var totalPaid = await _unitOfWork.Installments.GetTotalPaidAmountAsync(contract.Id);
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

                await _notificationService.SendNotificationsForInstallmentCreate(nextInstallment);
                await _unitOfWork.Installments.AddAsync(nextInstallment);
            }
            _unitOfWork.Contracts.Update(contract);

            await _unitOfWork.Save();
            return installment;
        }
    }
}