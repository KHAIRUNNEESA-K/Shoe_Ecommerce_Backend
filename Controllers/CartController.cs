using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Cart;
using ONSTEPS_API.Response;
using ONSTEPS_API.Services.Cart;
using System.Security.Claims;

namespace ONSTEPS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="User")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cart;
        public CartController(ICartService cart)
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
        [HttpPatch("update-cart-item")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem([FromBody] AddToCartDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response =await _cart.UpdateCartItem(userId, dto.productId,dto.quantity );

            if (response.Success)
            {
                return Ok(response);
            }
            {
                return BadRequest(response);
            }

        }
        [HttpDelete("delete-cart-item")]
        public async Task<ActionResult> DeleteCartItems([FromBody] CartDeleteRequestDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var response = await _cart.DeleteCart(userId, request.ProductId);

                if (response.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

    }
}
