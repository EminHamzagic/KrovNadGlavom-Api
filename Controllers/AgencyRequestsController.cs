using krov_nad_glavom_api.Application.Commands.AgencyRequests;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgencyRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgencyRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAgencyRequest(AgencyRequestToAddDto dto)
        {
            try
            {
                var command = new CreateAgencyRequestCommand(dto);
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