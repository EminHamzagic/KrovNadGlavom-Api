using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class UpdateUserCommand : IRequest<UserToReturnDto>
    {
        public string Id { get; set; }
        public UserToUpdateDto UserToUpdateDto { get; }
        public UpdateUserCommand(string id, UserToUpdateDto userToUpdateDto)
        {
            UserToUpdateDto = userToUpdateDto;
            Id = id;
        }
    }
}