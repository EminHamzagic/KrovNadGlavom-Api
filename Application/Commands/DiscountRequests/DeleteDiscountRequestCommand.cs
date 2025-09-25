using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class DeleteDiscountRequestCommand : IRequest<bool>
    {
		public string Id { get; }
        public DeleteDiscountRequestCommand(string id)
        {
			Id = id;
		}
	}
}