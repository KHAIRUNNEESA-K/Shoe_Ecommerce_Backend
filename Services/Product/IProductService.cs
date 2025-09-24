using ONSTEPS_API.DTO.Product;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Product
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDto>> AddProduct(ProductDto addproduct);
        Task<ApiResponse<List<ProductDto>>> GetAllProduct();
        Task<ApiResponse<ProductDto>> GetProductById(int id);
        Task<ApiResponse<DeleteProductDto>> DeleteProduct(int id);
        Task<ApiResponse<ProductUpdateDto>> UpdateProducts(int id, ProductUpdateDto product);
        Task<ApiResponse<List<ProductDto>>> SearchProducts(string search);
        Task<ApiResponse<PaginationDto<ProductDto>>> PaginationedProduct(int pageNumber, int pageSize);
    }
}
