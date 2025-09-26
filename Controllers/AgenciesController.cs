using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using krov_nad_glavom_api.Application.Commands.Agencies;
using krov_nad_glavom_api.Application.Queries.Agencies;
using krov_nad_glavom_api.Application.Utils;
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

        [AllowAnonymous]
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

        [HttpGet]
        public async Task<IActionResult> GetAllAgencies([FromQuery] QueryStringParameters parameters)
        {
            try
            {
                var command = new GetAllAgenciesQuery(parameters);
                var res = await _mediator.Send(command);
                Response.Headers.Append("X-Pagination", res.getMetadata());
                return Ok(res.Items);
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var command = new GetAgencyByIdQuery(id, userId);
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
        
        [AllowAnonymous]
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