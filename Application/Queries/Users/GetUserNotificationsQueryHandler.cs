using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, List<Notification>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserNotificationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<List<Notification>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Notifications.GetNotificationsByUserId(request.userId);
        }
    }
}