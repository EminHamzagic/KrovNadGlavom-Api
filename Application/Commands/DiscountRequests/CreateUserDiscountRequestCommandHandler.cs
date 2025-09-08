using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class CreateUserDiscountRequestCommandHandler : IRequestHandler<CreateUserDiscountRequestCommand, DiscountRequest>
    {
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public CreateUserDiscountRequestCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}

        public async Task<DiscountRequest> Handle(CreateUserDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            bool canAdd = await _unitofWork.DiscountRequests.CheckForExistingRequest(request.DiscountRequestToAddDto);
            if (!canAdd)
                throw new Exception("Već ste poslali zahtev za popust za ovaj stan");

            var discountRequest = _mapper.Map<DiscountRequest>(request.DiscountRequestToAddDto);
            discountRequest.Id = Guid.NewGuid().ToString();
            _unitofWork.DiscountRequests.AddAsync(discountRequest);
            await _unitofWork.Save();

            return discountRequest;
        }
    }
}