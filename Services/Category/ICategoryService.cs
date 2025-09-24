using ONSTEPS_API.DTO.Category;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Category
{
    public interface ICategoryService
    {
        Task<ApiResponse<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto);
        Task<ApiResponse<List<CategoryDto>>> GetCatogory();
        Task<ApiResponse<CategoryDto>> UpdateCategory(int id,CategoryDto category);
        Task<ApiResponse<CategoryDto>> DeleteCategory(int id);
        Task<ApiResponse<List<SearchByCategoryDto>>> SearchbyCategory(string searchcategory);
    }
}
