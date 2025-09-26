using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public UserChangePasswordDto ChangePasswordDto { get; set; }

        public ChangePasswordCommand(string userId, UserChangePasswordDto dto)
        {
            UserId = userId;
            ChangePasswordDto = dto;
        }
    }
}
