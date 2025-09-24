using ONSTEPS_API.DTO.Product;
using ONSTEPS_API.DTO.Wishlist;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Wishlist
{
    public interface IWishlistService
    {
        Task<ApiResponse<List<WishlistProductDto>>> AddWishlist(int userId,int productId);
        Task<ApiResponse<List<WishlistProductDto>>> GetWishlist(int userId);
        Task<ApiResponse<DeleteWishlistDto>> DeleteWishlist(int userId,int productId);
    }
}
