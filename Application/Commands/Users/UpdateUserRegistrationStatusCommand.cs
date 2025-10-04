using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class UpdateUserRegistrationStatusCommand : IRequest<bool>
    {
        public UserRegStatusUpdateDto UserRegStatusUpdateDto { get; }
		public string Origin { get; }
		public string Id { get; set; }
        public UpdateUserRegistrationStatusCommand(string id, UserRegStatusUpdateDto userRegStatusUpdateDto, string origin)
        {
            Id = id;
            UserRegStatusUpdateDto = userRegStatusUpdateDto;
			Origin = origin;
		}
    }
}