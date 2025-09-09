using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class UpdateDiscountRequestCommandHandler : IRequestHandler<UpdateDiscountRequestCommand, DiscountRequest>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public UpdateDiscountRequestCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<DiscountRequest> Handle(UpdateDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            var discountRequest = await _unitofWork.DiscountRequests.GetByIdAsync(request.Id);
            if (discountRequest == null)
                throw new Exception("Zahtev za popust nije pronađen");

            _mapper.Map(request.DiscountRequestToUpdateDto, discountRequest);
            await _unitofWork.Save();

            return discountRequest;
        }
    }
}