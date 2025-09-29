using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class DeleteDiscountRequestCommandHandler : IRequestHandler<DeleteDiscountRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteDiscountRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            var discountRequest = await _unitOfWork.DiscountRequests.GetByIdAsync(request.Id);
            if (discountRequest == null)
                throw new Exception("Zahtev za popust nije pronađen");

            _unitOfWork.DiscountRequests.Remove(discountRequest);
            await _unitOfWork.Save();

            return true;
        }
    }
}