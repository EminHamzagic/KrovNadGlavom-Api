using krov_nad_glavom_api.Application.Commands.Buildings;
using krov_nad_glavom_api.Application.Queries.Buildings;
using krov_nad_glavom_api.Data.DTO.Building;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleBuilding(string id)
        {
            try
            {
                var command = new GetBuildingByIdQuery(id);
                var building = await _mediator.Send(command);
                return Ok(building);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("company/{companyId}")]
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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuilding(string id, BuildingToUpdateDto dto)
        {
            try
            {
                var command = new UpdateBuildingCommand(id, dto);
                var building = await _mediator.Send(command);
                return Ok(building);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(string id)
        {
            try
            {
                var command = new DeleteBuildingCommand(id);
                var building = await _mediator.Send(command);
                return Ok(building);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}