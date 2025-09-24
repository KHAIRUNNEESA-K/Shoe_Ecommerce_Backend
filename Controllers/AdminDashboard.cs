using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Admin;
using ONSTEPS_API.Response;
using ONSTEPS_API.Services.Admin;

namespace ONSTEPS_API.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDashboard : ControllerBase
    {
        private readonly IAdminDashboardService _adminServices;
        public AdminDashboard(IAdminDashboardService adminServices)
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

        #region Block User By ID
        [AllowAnonymous]
        [HttpPut("user/block/{id}")]
        public async Task<ActionResult> BlockUser(int id)
        {
            try
            {
                var response = await _adminServices.BlockUser(id);

                if (response == null)
                {
                    return NotFound(new ApiResponse<dynamic>
                    {
                        Success = false,
                        Message = "User not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<dynamic>
                {
                    Success = true,
                    Message = "The user has been successfully blocked.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                });
            }
        }

        #endregion

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
        [HttpGet("TotalRevenue")]
        public async Task<ActionResult> TotalRevenue()
        {
            var response=await _adminServices.TotalRevenue();
            if (response==null)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
        [HttpGet("TotalRevenue-By-date")]
        public async Task<ActionResult> RevenueByDate()
        {
            var response = await _adminServices.TotalRevenueByDate();
            if (response == null || response.Data == null || !response.Data.Any())
            {
                return NotFound(response);
            }
            return Ok(response);
        }


    }

}
