using krov_nad_glavom_api.Application.Commands.DiscountRequests;
using krov_nad_glavom_api.Application.Queries.DiscountRequests;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiscountRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUserDiscountRequest(DiscountRequestToAddDto dto)
        {
            try
            {
                var command = new CreateUserDiscountRequestCommand(dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserDiscountRequest(string id, [FromQuery] string status)
        {
            try
            {
                var command = new GetUserDiscountRequestsQuery(id, status);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("agency/{id}")]
        public async Task<IActionResult> GetAgencyDiscountRequest(string id, [FromQuery] string status)
        {
            try
            {
                var command = new GetAgencyDiscountRequestsQuery(id, status);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompanyDiscountRequest(string id, [FromQuery] string status)
        {
            try
            {
                var command = new GetCompanyDiscountRequestsQuery(id, status);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscountRequest(string id, DiscountRequestToUpdateDto dto)
        {
            try
            {
                var command = new UpdateDiscountRequestCommand(id, dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscountRequest(string id)
        {
            try
            {
                var command = new DeleteDiscountRequestCommand(id);
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