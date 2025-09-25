using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using krov_nad_glavom_api.Application.Commands.Apartments;
using krov_nad_glavom_api.Application.Queries.Apartments;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateApartment(ApartmentToAddDto dto)
        {
            try
            {
                var command = new CreateApartmentCommand(dto);
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("multiple")]
        public async Task<IActionResult> CreateMultipleApartment(MultipleApartmentsToAddDto dto)
        {
            try
            {
                var command = new CreateMultipleApartmentsCommand(dto);
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartmentById(string id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var command = new GetApartmentByIdQuery(id, userId);
                var apartment = await _mediator.Send(command);
                return Ok(apartment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAvailableApartments([FromQuery] QueryStringParameters parameters)
        {
            try
            {
                var command = new GetAvailableApartmentsQuery(parameters);
                var res = await _mediator.Send(command);
                Response.Headers.Append("X-Pagination", res.getMetadata());
                return Ok(res.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApartment(string id, ApartmentToUpdateDto dto)
        {
            try
            {
                var command = new UpdateApartmentCommand(dto, id);
                var apartment = await _mediator.Send(command);
                return Ok(apartment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(string id)
        {
            try
            {
                var command = new DeleteApartmentCommand(id);
                var apartment = await _mediator.Send(command);
                return Ok(apartment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}