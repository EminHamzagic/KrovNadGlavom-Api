using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class DeleteUserCommand : IRequest<bool>
    {
		public string Id { get; }
        public DeleteUserCommand(string id)
        {
			Id = id;
		}
	}
}