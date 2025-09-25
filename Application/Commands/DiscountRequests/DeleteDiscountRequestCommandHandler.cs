using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class DeleteDiscountRequestCommandHandler : IRequestHandler<DeleteDiscountRequestCommand, bool>
    {
        private readonly IUnitofWork _unitofWork;
        public DeleteDiscountRequestCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<bool> Handle(DeleteDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            var discountRequest = await _unitofWork.DiscountRequests.GetByIdAsync(request.Id);
            if (discountRequest == null)
                throw new Exception("Zahtev za popust nije pronađen");

            _unitofWork.DiscountRequests.Remove(discountRequest);
            await _unitofWork.Save();

            return true;
        }
    }
}