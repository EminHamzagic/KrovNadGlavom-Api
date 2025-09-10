using krov_nad_glavom_api.Application.Commands.Contracts;
using krov_nad_glavom_api.Application.Queries.Contracts;
using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(ContractToAddDto dto)
        {
            try
            {
                var command = new CreateContractCommand(dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleContract(string id)
        {
            try
            {
                var command = new GetContractByIdQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserContracts(string id)
        {
            try
            {
                var command = new GetUserContractsQuery(id);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("agency/{id}")]
        public async Task<IActionResult> GetAgencyContracts(string id)
        {
            try
            {
                var command = new GetAgencyContractsQuery(id);
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