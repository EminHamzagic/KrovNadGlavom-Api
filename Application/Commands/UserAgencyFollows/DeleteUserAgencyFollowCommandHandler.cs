using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.UserAgencyFollows
{
    public class DeleteUserAgencyFollowCommandHandler : IRequestHandler<DeleteUserAgencyFollowCommand, UserAgencyFollow>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserAgencyFollowCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserAgencyFollow> Handle(DeleteUserAgencyFollowCommand request, CancellationToken cancellationToken)
        {
            var userFollow = await _unitOfWork.UserAgencyFollows.GetByIdAsync(request.Id);
            if (userFollow == null)
                throw new Exception("Praćenje korisnika nije pronađeno");

            _unitOfWork.UserAgencyFollows.Remove(userFollow);
            await _unitOfWork.Save();

            return userFollow;
        }
    }
}