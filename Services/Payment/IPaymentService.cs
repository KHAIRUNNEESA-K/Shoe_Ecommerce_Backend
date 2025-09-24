using ONSTEPS_API.DTO.Payment;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Payment
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentResponseDto>> CreatePayment(int userId, int orderId);
        Task<ApiResponse<PaymentResponseDto>> VerifyPayment(int userId, VerifyPaymentDto dto);
    }
}
