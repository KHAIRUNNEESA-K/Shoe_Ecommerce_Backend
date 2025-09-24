using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Address;
using ONSTEPS_API.Models;
using ONSTEPS_API.Response;
using ONSTEPS_API.Services.Address;
using System.Security.Claims;

namespace ONSTEPS_API.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]

    public class AddressController : ControllerBase
    {
        private readonly IAddress _address;
        public AddressController(IAddress address)
        {
            _address = address;
        }
        [HttpPost("Add-Address")]
        [AllowAnonymous]
        public async Task<ActionResult> AddAddress(AddressRequestDto addressDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _address.AddAddress(userId, addressDto);
                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        [HttpGet("Get-Address")]
        public async Task<ActionResult> GetAddress()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _address.GetAddress(userId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("GetAddressById/{id}")]
        public async Task<ActionResult> GetAddressById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _address.GetAddressById(userId, id);

            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        [HttpPut("UpdateAdress")]
        public async Task<ActionResult> UpdateAddress(int Addressid,[FromBody] AddressRequestDto addressDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _address.UpdateAddress(userId,Addressid,addressDto);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpDelete("DeleteAddress")]
        public async Task<ActionResult> DeleteAddress(int AddressId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response=await _address.DeleteAddress(userId,AddressId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
