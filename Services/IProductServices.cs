using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface IProductServices
    {
        Task<ApiResponse<ProductDto>> AddProduct(ProductDto addproduct);
        Task<ApiResponse<List<ProductDto>>> GetAllProduct();
        Task<ApiResponse<ProductDto>> GetProductById(int id);
     
    }
}
