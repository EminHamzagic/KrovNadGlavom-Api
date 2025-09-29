using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public class GetAgencyFollowersQueryHandler : IRequestHandler<GetAgencyFollowersQuery, List<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAgencyFollowersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> Handle(GetAgencyFollowersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserAgencyFollows.GetAgencyFollowers(request.Id);
            return users;
        }
    }
}