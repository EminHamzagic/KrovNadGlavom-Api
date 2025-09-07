using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.UserAgencyFollows
{
    public class DeleteUserAgencyFollowCommand : IRequest<UserAgencyFollow>
    {
		public string Id { get; }
        public DeleteUserAgencyFollowCommand(string id)
        {
			Id = id;
		}
	}
}