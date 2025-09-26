using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class ForwardDiscountRequestCommand : IRequest<string>
    {
		public string RequestId { get; }
        public ForwardDiscountRequestCommand(string requestId)
        {
			RequestId = requestId;
		}
	}
}