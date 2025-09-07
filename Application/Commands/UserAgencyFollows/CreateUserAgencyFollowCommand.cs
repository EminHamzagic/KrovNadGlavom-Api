using krov_nad_glavom_api.Data.DTO.UserAgencyFollow;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.UserAgencyFollows
{
    public class CreateUserAgencyFollowCommand : IRequest<UserAgencyFollow>
    {
		public UserAgencyFollowToAddDto UserAgencyFollowToAddDto { get; }
        public CreateUserAgencyFollowCommand(UserAgencyFollowToAddDto userAgencyFollowToAddDto)
        {
			UserAgencyFollowToAddDto = userAgencyFollowToAddDto;
		}
	}
}