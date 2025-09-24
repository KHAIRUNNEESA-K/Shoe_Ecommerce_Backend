using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Payment;
using ONSTEPS_API.Models;
using ONSTEPS_API.Services.Payment;
using System.Security.Claims;

namespace ONSTEPS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="User")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> CreatePayment([FromBody] PaymentRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { success = false, message = "Invalid or missing user ID in token." });

            var response = await _paymentService.CreatePayment(userId, request.OrderId);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }


        [HttpPost("Verify")]
        public async Task<ActionResult> VerifyPayment([FromBody] VerifyPaymentDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { success = false, message = "Invalid or missing user ID in token." });

            var response = await _paymentService.VerifyPayment(userId, dto);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
