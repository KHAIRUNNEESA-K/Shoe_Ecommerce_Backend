using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.Models;
using ONSTEPS_API.Services;
using System.Security.Claims;

namespace ONSTEPS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlist _wishlist;
        public WishlistController(IWishlist wishlist)
        {
            _wishlist = wishlist;
        }
        [HttpPost("AddWishlist")]
        public async Task<ActionResult> AddToWishlist(int ProductId)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "Admin")
            {
                return StatusCode(403);
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _wishlist.AddWishlist(userId, ProductId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("GetWishlist")]
        public async Task<ActionResult> GetWishlistProduct()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _wishlist.GetWishlist(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpDelete("Delete_from_Wishlist")]
        public async Task<ActionResult> DeleteWishlist(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _wishlist.DeleteWishlist(userId, productId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


    }
}
