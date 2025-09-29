using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class VerifyUserEmailCommand : IRequest<bool>
    {
		public string Token { get; }
        public VerifyUserEmailCommand(string token)
        {
			Token = token;
		}
	}
}