using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Services;
using System.Security.Claims;

namespace ONSTEPS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="User")]
    public class CartController : ControllerBase
    {
        private readonly ICart _cart;
        public CartController(ICart cart)
        {
            _cart = cart;
        }
        [HttpPost("AddToCart")]
        public async Task<ActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _cart.AddToCart(userId, dto.productId, dto.quantity);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("Cart_Items")]
        public async Task<ActionResult> GetCartItem()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response=await _cart.GetCartItems(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpDelete("DeleteCart")]
        public async Task<ActionResult> DeleteCartItems([FromBody] int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _cart.DeleteCart(userId, productId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
