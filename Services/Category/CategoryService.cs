using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Category;
using ONSTEPS_API.DTO.Wishlist;
using ONSTEPS_API.Response;
using System.Data;
using System.Runtime.InteropServices;

namespace ONSTEPS_API.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly string _connectionString;
        public CategoryService(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ApiResponse<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto)
        {
            using (var conncetion = new SqlConnection(_connectionString))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@name", categoryDto.CategoryName);
                parameter.Add("@isactive", true);
                parameter.Add("@isdeleted", false);
                parameter.Add("@createdat", DateTime.Now);
                parameter.Add("@updatedat", null);
                parameter.Add("@deletedAt", null);
                parameter.Add("@Flag", 400);

                try
                {

                    await conncetion.ExecuteAsync("SP_CategoryManagement", parameter, commandType: CommandType.StoredProcedure);
                    var addcategory = new AddCategoryDto
                    {
                        CategoryName= categoryDto.CategoryName,
                    };
                    return new ApiResponse<AddCategoryDto>
                    {
                        Success = true,
                        Message="Category Added Successfully",
                        Data=addcategory

                    };
                }
                catch (Exception ex)
                {
                    return new ApiResponse<AddCategoryDto>
                    {
                        Success = false,
                        Message=$"Failed to add category {ex.Message}",
                        Data=null
                    };

                }
            }
        }
        public async Task<ApiResponse<List<CategoryDto>>> GetCatogory()
        {
            var parameter =new DynamicParameters();
            parameter.Add("@Flag", 401);
            using (var connection = new SqlConnection(_connectionString))
            {
                var getCategories = await connection.QueryAsync<CategoryDto>("SP_CategoryManagement",parameter, commandType: CommandType.StoredProcedure);

                return new ApiResponse<List<CategoryDto>>
                {
                    Success = true,
                    Message="Catogory Detailes",
                    Data=getCategories.ToList()
                };
            }
        }
        public async Task<ApiResponse<CategoryDto>> UpdateCategory(int id, CategoryDto category)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            parameter.Add("@name", category.CategoryName);
            parameter.Add("@isactive", category.IsActive);
            parameter.Add("@Flag", 402);

            using (var connection = new SqlConnection(_connectionString))
            {
                var updateCategory = await connection.QueryFirstOrDefaultAsync<CategoryDto>("SP_CategoryManagement", parameter, commandType: CommandType.StoredProcedure);
                if (updateCategory == null)
                {
                    return new ApiResponse<CategoryDto>
                    {
                        Success = false,
                        Message="category not found",
                        Data=null
                    };

                }
                else
                {
                    return new ApiResponse<CategoryDto>
                    {
                        Success = true,
                        Message="Category Update Succesfully",
                        Data=updateCategory
                    };
                }

            }
        }
        public async Task<ApiResponse<CategoryDto>> DeleteCategory(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            parameter.Add("@Flag", 403);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try {
                    var deleteCategory = await connection.QueryFirstOrDefaultAsync<CategoryDto>("SP_CategoryManagement", parameter, commandType: CommandType.StoredProcedure);
                    if (deleteCategory != null)
                    {
                        return new ApiResponse<CategoryDto>
                        {
                            Success = true,
                            Message="Product successfully remove from Wishlist",
                            Data=deleteCategory
                        };
                    }
                    else
                    {
                        return new ApiResponse<CategoryDto>
                        {
                            Success=false,
                            Message="Error",
                            Data=null
                        };
                    }
                }
                
         
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }

    }
        
        public async Task<ApiResponse<List<SearchByCategoryDto>>> SearchbyCategory(string searchcategory)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CategoryName", searchcategory);
            parameter.Add("@Flag", 306);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cProducts = await connection.QueryAsync<SearchByCategoryDto>("SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure);
                    var searchByCategory = cProducts.ToList();
                    if (!searchByCategory.Any())
                    {
                        return new ApiResponse<List<SearchByCategoryDto>>
                        {
                            Success=false,
                            Message="product not found",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<SearchByCategoryDto>>
                        {
                            Success=true,
                            Message="Search Product",
                            Data=searchByCategory
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<SearchByCategoryDto>>
                {
                    Success=false,
                    Message=ex.Message,
                    Data=null
                };

            }

        }


    }
}
