using krov_nad_glavom_api.Application.Commands.ConstructionCompanies;
using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
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
    }
}