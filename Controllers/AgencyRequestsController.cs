using krov_nad_glavom_api.Application.Commands.AgencyRequests;
using krov_nad_glavom_api.Application.Queries.AgencyRequests;
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
        
        [HttpGet("agency/{id}")]
        public async Task<IActionResult> GetAgencyRequestsByAgencyId(string id)
        {
            try
            {
                var command = new GetAgencyRequestsByAgencyIdQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetAgencyRequestsByComapnyId(string id)
        {
            try
            {
                var command = new GetAgencyRequestsByCompanyIdQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgencyRequest(string id, AgencyRequestToUpdateDto dto)
        {
            try
            {
                var command = new UpdateAgencyRequestCommand(id, dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgencyRequest(string id)
        {
            try
            {
                var command = new DeleteAgencyRequestCommand(id);
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