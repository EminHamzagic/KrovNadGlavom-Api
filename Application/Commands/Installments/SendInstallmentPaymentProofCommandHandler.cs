using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class SendInstallmentPaymentProofCommandHandler : IRequestHandler<SendInstallmentPaymentProofCommand, string>
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICloudinaryService _cloudinaryService;

        public SendInstallmentPaymentProofCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
			_unitOfWork = unitOfWork;
			_cloudinaryService = cloudinaryService;
        }

        public async Task<string> Handle(SendInstallmentPaymentProofCommand request, CancellationToken cancellationToken)
        {
            var installment = await _unitOfWork.Installments.GetByIdAsync(request.InstallmentProofToSendDto.Id);
            if (installment == null)
                throw new Exception("Rata nije pronađena");

            if (!string.IsNullOrEmpty(installment.PaymentProof))
            {
                await _cloudinaryService.DeleteImageAsync(installment.PaymentProof);
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(request.InstallmentProofToSendDto.File, "KrovNadGlavom");
            installment.PaymentProof = imageUrl;
            _unitOfWork.Installments.Update(installment);
            await _unitOfWork.Save();

            return imageUrl;
        }
    }
}