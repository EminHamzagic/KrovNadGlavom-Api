using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.UserAgencyFollows
{
    public class DeleteUserAgencyFollowCommandHandler : IRequestHandler<DeleteUserAgencyFollowCommand, UserAgencyFollow>
    {
        private readonly IUnitofWork _unitofWork;
        public DeleteUserAgencyFollowCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<UserAgencyFollow> Handle(DeleteUserAgencyFollowCommand request, CancellationToken cancellationToken)
        {
            var userFollow = await _unitofWork.UserAgencyFollows.GetByIdAsync(request.Id);
            if (userFollow == null)
                throw new Exception("Praćenje korisnika nije pronađeno");

            _unitofWork.UserAgencyFollows.Remove(userFollow);
            await _unitofWork.Save();

            return userFollow;
        }
    }
}