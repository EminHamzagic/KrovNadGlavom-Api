using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class GetUserFollowingsQueryHandler : IRequestHandler<GetUserFollowingsQuery, List<Agency>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetUserFollowingsQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<List<Agency>> Handle(GetUserFollowingsQuery request, CancellationToken cancellationToken)
        {
            var agencies = await _unitofWork.UserAgencyFollows.GetUserFollowing(request.Id);
            return agencies;
        }
    }
}