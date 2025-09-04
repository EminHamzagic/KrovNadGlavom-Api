using krov_nad_glavom_api.Application.Commands.Apartments;
using krov_nad_glavom_api.Application.Queries.Apartments;
using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
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
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartmentById(string id)
        {
            try
            {
                var command = new GetApartmentByIdQuery(id);
                var apartment = await _mediator.Send(command);
                return Ok(apartment);
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