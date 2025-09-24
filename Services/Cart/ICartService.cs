
using ONSTEPS_API.DTO.Cart;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Cart
{
    public interface ICartService
    {
        Task<ApiResponse<List<CartItemDto>>> AddToCart(int userId,int productId,int quantity);
        Task<ApiResponse<List<CartItemDto>>> GetCartItems(int userId);
        Task<ApiResponse<List<CartItemDto>>> UpdateCartItem(int userId,int productId,int quantty);
        Task<ApiResponse<DeleteCartDto>> DeleteCart(int userId,int productId);
    }
}
