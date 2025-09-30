using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class RequestPasswordResetCommand : IRequest<bool>
    {
		public UserPasswordResetRequestDto UserPasswordResetRequestDto { get; }
        public RequestPasswordResetCommand(UserPasswordResetRequestDto userPasswordResetDto)
        {
			UserPasswordResetRequestDto = userPasswordResetDto;
		}
	}
}