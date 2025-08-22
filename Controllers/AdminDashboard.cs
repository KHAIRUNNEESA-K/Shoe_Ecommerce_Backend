using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.Services;

namespace ONSTEPS_API.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDashboard : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        public AdminDashboard(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        [HttpGet("users")]
        public async Task<ActionResult> GetAll()
        {
            var response= await _adminServices.GetAllUsers();
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("user/{id}")]
        public async Task<ActionResult> GetBy(int id)
        {
            var respons =await  _adminServices.GetById(id);
            if (respons == null)
            {
                return BadRequest(respons);
            }
            return Ok(respons);
        }
        [HttpPut("user/block{id}")]
        public async Task<ActionResult> Blockuser(int id)
        {
            var response=await _adminServices.BlockUser(id);
            if (response==null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPut("user/unblock{id}")]
        public async Task<ActionResult> Unblockuser(int id)
        {
            var response=await _adminServices.UnBlockUser(id);
            if(response==null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }

}
