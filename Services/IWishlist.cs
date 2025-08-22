using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface IWishlist
    {
        Task<ApiResponse<List<ProductDto>>> AddWishlist(int userId,int productId);
        Task<ApiResponse<List<ProductDto>>> GetWishlist(int userId);
        Task<ApiResponse<DeleteWishlistDto>> DeleteWishlist(int userId,int productId);
    }
}
