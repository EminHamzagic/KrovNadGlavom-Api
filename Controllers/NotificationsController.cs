using krov_nad_glavom_api.Application.Commands.Notifications;
using krov_nad_glavom_api.Application.Queries.Users;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetUserNotifications(string id)
        {
            try
            {
                var command = new GetUserNotificationsQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUserNotification(string id)
        {
            try
            {
                var command = new DeleteNotificationCommand(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}