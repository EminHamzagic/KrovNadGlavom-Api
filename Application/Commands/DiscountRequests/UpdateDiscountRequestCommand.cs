using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class UpdateDiscountRequestCommand : IRequest<DiscountRequest>
    {
		public string Id { get; }
		public DiscountRequestToUpdateDto DiscountRequestToUpdateDto { get; }
        public UpdateDiscountRequestCommand(string id, DiscountRequestToUpdateDto discountRequestToUpdateDto)
        {
			Id = id;
			DiscountRequestToUpdateDto = discountRequestToUpdateDto;
		}
	}
}