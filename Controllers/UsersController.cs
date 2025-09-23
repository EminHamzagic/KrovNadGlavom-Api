using krov_nad_glavom_api.Application.Commands.Users;
using krov_nad_glavom_api.Application.Queries.Users;
using krov_nad_glavom_api.Data.DTO.Google;
using krov_nad_glavom_api.Data.DTO.Installment;
using krov_nad_glavom_api.Data.DTO.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace krov_nad_glavom_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserToReturnDto>> Login(UserToLoginDto dto)
        {
            try
            {
                var command = new LoginUserCommand(dto);
                var user = await _mediator.Send(command);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<ActionResult<UserToReturnDto>> GoogleLogin(GoogleAuthRequestDto dto)
        {
            try
            {
                var command = new GoogleLoginUserCommand(dto);
                var user = await _mediator.Send(command);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(UserToAddDto dto)
        {
            var command = new CreateUserCommand(dto);
            var userId = await _mediator.Send(command);
            return Ok(userId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserToReturnDto>> GetUser(string id)
        {
            try
            {
                var command = new GetUserByIdQuery(id);
                var user = await _mediator.Send(command);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/followings")]
        public async Task<IActionResult> GetUserFollowings(string id)
        {
            try
            {
                var command = new GetUserFollowingsQuery(id);
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
                var command = new SetUserProfileImageCommand(dto);
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