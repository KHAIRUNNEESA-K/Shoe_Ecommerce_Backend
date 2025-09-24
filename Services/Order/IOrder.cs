using ONSTEPS_API.DTO.Order;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Order
{
    public interface IOrder
    {
        Task<ApiResponse<OrderDto>> PlaceOrder(int userId,OrderRequestDto orderDto);
        Task<ApiResponse<List<OrderDto>>> GetOrders(int userId);
        Task<ApiResponse<OrderDetailsDto>> GetOrderDetails(int userId, int orderId);
        Task<ApiResponse<OrderStatusDto>> UpdateOrderStatus(int userId,int orderId, String status);
    }
}
