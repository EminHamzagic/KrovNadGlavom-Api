using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Notifications
{
    public class DeleteNotificationCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public DeleteNotificationCommand(string id)
        {
            Id = id;
        }
    }
}