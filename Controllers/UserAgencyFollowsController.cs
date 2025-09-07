using krov_nad_glavom_api.Application.Commands.UserAgencyFollows;
using krov_nad_glavom_api.Data.DTO.UserAgencyFollow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserAgencyFollowsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserAgencyFollowsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAgencyFollow(UserAgencyFollowToAddDto dto)
        {
            try
            {
                var command = new CreateUserAgencyFollowCommand(dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAgencyFollow(string id)
        {
            try
            {
                var command = new DeleteUserAgencyFollowCommand(id);
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