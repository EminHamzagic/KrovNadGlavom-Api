using krov_nad_glavom_api.Application.Commands.Installments;
using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InstallmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstallmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateInstallment(InstallmentToAddDto dto)
        {
            try
            {
                var command = new CreateInstallmentCommand(dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("proof")]
        public async Task<IActionResult> CreateInstallment([FromForm] InstallmentProofToSendDto dto)
        {
            try
            {
                var command = new SendInstallmentPaymentProofCommand(dto);
                var res = await _mediator.Send(command);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> ConfirmInstallment(string id)
        {
            try
            {
                var command = new ConfirmInstallmentCommand(id);
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