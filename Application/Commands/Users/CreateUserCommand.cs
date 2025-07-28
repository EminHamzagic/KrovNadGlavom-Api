using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<string>
    {
        public UserToAddDto UserToAddDto;
        public CreateUserCommand(UserToAddDto userToAddDto)
        {
            UserToAddDto = userToAddDto;
        }
    }
}