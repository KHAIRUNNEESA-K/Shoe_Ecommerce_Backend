using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Order;
using ONSTEPS_API.Services.Order;
using System.Security.Claims;

namespace ONSTEPS_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;
        public OrderController(IOrder order)
        {
            _order = order;
        }
        [Authorize(Roles = "User")]
        [HttpPost("PlaceOrder")]
        public async Task<ActionResult> PlaceOrder(OrderRequestDto orderDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID in token." });
                }

                var response = await _order.PlaceOrder(userId, orderDto);

                if (response.Success)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("Ordered-Items")]
        public async Task<ActionResult> GetOrder()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid or missing user ID in token." });
            }

            var response = await _order.GetOrders(userId);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
        [HttpGet("Order-Details/{orderId}")]
        public async Task<ActionResult> GetOrderDetails(int orderId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _order.GetOrderDetails(userId, orderId);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
        [Authorize(Roles ="Admin")]
        [HttpPatch("Order-Status")]
        public async Task<ActionResult> UpdateStatus([FromBody] OrderStatusDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _order.UpdateOrderStatus(userId, dto.OrderId, dto.Status);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }



    }
}