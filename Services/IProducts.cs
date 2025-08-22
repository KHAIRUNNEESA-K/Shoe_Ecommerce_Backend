using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface IProducts
    {
        Task<ApiResponse<DeleteProductDto>> DeleteProduct(int id);
        Task<ApiResponse<ProductUpdateDto>> UpdateProducts(int id,ProductUpdateDto product);
        Task<ApiResponse<List<ProductDto>>> SearchProducts(string search);
        Task<ApiResponse<PaginationDto<ProductDto>>> PaginationedProduct(int pageNumber,int pageSize);

    }
}
