using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class UpdateDiscountRequestCommandHandler : IRequestHandler<UpdateDiscountRequestCommand, DiscountRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDiscountRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiscountRequest> Handle(UpdateDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            var discountRequest = await _unitOfWork.DiscountRequests.GetByIdAsync(request.Id);
            if (discountRequest == null)
                throw new Exception("Zahtev za popust nije pronađen");

            _mapper.Map(request.DiscountRequestToUpdateDto, discountRequest);
            _unitOfWork.DiscountRequests.Update(discountRequest);
            await _unitOfWork.Save();

            return discountRequest;
        }
    }
}