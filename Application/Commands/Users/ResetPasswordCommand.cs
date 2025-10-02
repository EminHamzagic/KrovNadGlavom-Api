using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public UserPasswordResetDto UserPasswordResetDto { get; set; }
        public ResetPasswordCommand(UserPasswordResetDto userPasswordRequestDto)
        {
            this.UserPasswordResetDto = userPasswordRequestDto;
		}
	}
}