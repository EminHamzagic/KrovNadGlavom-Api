using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users.LoginUser
{
    public class LoginUserCommand : IRequest<UserToReturnDto>
    {
        public UserToLoginDto UserToLoginDto;
        public LoginUserCommand(UserToLoginDto userToLoginDto)
        {
            UserToLoginDto = userToLoginDto;
        }
    }
}