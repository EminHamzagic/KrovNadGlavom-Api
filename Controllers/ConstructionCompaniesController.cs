using krov_nad_glavom_api.Application.Commands.ConstructionCompanies;
using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConstructionCompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConstructionCompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateCompany(ConstructionCompanyToAddDto dto)
        {
            try
            {
                var command = new CreateConstructionCompanyCommand(dto);
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(string id, ConstructionCompanyToUpdateDto dto)
        {
            try
            {
                var command = new UpdateConstructionCompanyCommand(id, dto);
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
                var command = new SetCompanyImageCommand(dto);
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