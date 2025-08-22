using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;
using ONSTEPS_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ONSTEPS_API.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _userSignup;

        public AuthController(IAuthServices userSignup)
        {
            _userSignup = userSignup;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] SignupDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userSignup.Register(user);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto _user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _response = await _userSignup.Login(_user);
            if (_response.Success)
            {
                return Ok(_response);
            }
            return BadRequest(_response);
        }

    }
}
