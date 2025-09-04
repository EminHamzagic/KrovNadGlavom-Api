using krov_nad_glavom_api.Application.Commands.Buildings;
using krov_nad_glavom_api.Application.Queries.Buildings;
using krov_nad_glavom_api.Data.DTO.Building;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateBuilding(BuildingToAddDto dto)
        {
            try
            {
                var command = new CreateBuildingCommand(dto);
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("/company/{companyId}")]
        public async Task<ActionResult<Building>> GetCompanyBuildings(string companyId)
        {
            try
            {
                var command = new GetBuildingsByCompanyIdQuery(companyId);
                var buildings = await _mediator.Send(command);
                return Ok(buildings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}