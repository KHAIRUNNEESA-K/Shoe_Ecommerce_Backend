using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface ICart
    {
        Task<ApiResponse<List<CartItemDto>>> AddToCart(int userId,int productId,int quantity);
        Task<ApiResponse<List<CartItemDto>>> GetCartItems(int userId);
        Task<ApiResponse<DeleteCartDto>> DeleteCart(int userId,int productId);
    }
}
