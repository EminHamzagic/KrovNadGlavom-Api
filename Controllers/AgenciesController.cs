using krov_nad_glavom_api.Application.Commands.Agencies;
using krov_nad_glavom_api.Application.Queries.Agencies;
using krov_nad_glavom_api.Data.DTO.Agency;
using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgenciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgenciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgencies()
        {
            try
            {
                var command = new GetAllAgenciesQuery();
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgencyById(string id)
        {
            try
            {
                var command = new GetAgencyByIdQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/followers")]
        public async Task<IActionResult> GetAgencyFollowers(string id)
        {
            try
            {
                var command = new GetAgencyFollowersQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAgency(AgencyToAddDto dto)
        {
            try
            {
                var command = new CreateAgencyCommand(dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgency(string id, AgencyToAddDto dto)
        {
            try
            {
                var command = new UpdateAgencyCommand(id, dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("image")]
        public async Task<IActionResult> SetUserPfp([FromForm] InstallmentProofToSendDto dto)
        {
            try
            {
                var command = new SetAgencyLogoCommand(dto);
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