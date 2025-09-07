using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public class GetAgencyFollowersQueryHandler : IRequestHandler<GetAgencyFollowersQuery, List<User>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetAgencyFollowersQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<List<User>> Handle(GetAgencyFollowersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitofWork.UserAgencyFollows.GetAgencyFollowers(request.Id);
            return users;
        }
    }
}