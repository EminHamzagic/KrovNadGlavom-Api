using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Reservations
{
    public class DeleteReservationCommand : IRequest<bool>
    {
		public string Id { get; }
        public DeleteReservationCommand(string id)
        {
			Id = id;
		}
	}
}