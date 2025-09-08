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
        
        [HttpGet("{id}/user")]
        public async Task<IActionResult> GetUserDiscountRequest(string id)
        {
            try
            {
                var command = new GetUserDiscountRequestsQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}/agency")]
        public async Task<IActionResult> GetAgencyDiscountRequest(string id)
        {
            try
            {
                var command = new GetAgencyDiscountRequestsQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}/company")]
        public async Task<IActionResult> GetCompanyDiscountRequest(string id)
        {
            try
            {
                var command = new GetCompanyDiscountRequestsQuery(id);
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