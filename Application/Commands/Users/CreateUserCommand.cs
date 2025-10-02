using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<string>
    {
        public UserToAddDto UserToAddDto;
        public string Origin { get; set; }
        public CreateUserCommand(UserToAddDto userToAddDto, string origin)
        {
            Origin = origin;
            UserToAddDto = userToAddDto;
        }
    }
}