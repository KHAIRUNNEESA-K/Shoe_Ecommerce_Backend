using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface ICategory
    {
        Task<ApiResponse<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto);
        Task<ApiResponse<List<CategoryDto>>> GetCatogory();
        Task<ApiResponse<CategoryDto>> UpdateCategory(int id,CategoryDto category);
        Task<ApiResponse<List<SearchByCategoryDto>>> SearchbyCategory(string searchcategory);
    }
}
