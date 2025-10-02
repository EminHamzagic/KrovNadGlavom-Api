using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Notifications
{
    public class GetNotificationCountQueryHandler : IRequestHandler<GetNotificationCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetNotificationCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.Notifications.GetUserNotificationsCount(request.userId);
            return count;
        }
    }
}