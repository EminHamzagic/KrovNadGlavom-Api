using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class CreateUserDiscountRequestCommand : IRequest<DiscountRequest>
    {
		public DiscountRequestToAddDto DiscountRequestToAddDto { get; }
        public CreateUserDiscountRequestCommand(DiscountRequestToAddDto discountRequestToAddDto)
        {
			DiscountRequestToAddDto = discountRequestToAddDto;
		}
	}
}