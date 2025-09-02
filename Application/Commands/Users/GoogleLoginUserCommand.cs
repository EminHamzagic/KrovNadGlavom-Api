using krov_nad_glavom_api.Data.DTO.Google;
using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class GoogleLoginUserCommand : IRequest<UserToReturnDto>
    {
		public GoogleAuthRequestDto GoogleAuthRequest;
		public GoogleLoginUserCommand(GoogleAuthRequestDto googleAuthRequest)
        {
			GoogleAuthRequest = googleAuthRequest;
		}
    }
}