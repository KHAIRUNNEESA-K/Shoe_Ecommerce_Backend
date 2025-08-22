using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;
using System.Data;
using System.Runtime.InteropServices;

namespace ONSTEPS_API.Services
{
    public class CategoryServices : ICategory
    {
        private readonly string _connectionString;
        public CategoryServices(IConfiguration configuration)
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
                parameter.Add("@updatedat", (DateTime?)null);
                parameter.Add("@deletedAt", (DateTime?)null);

                try
                {

                    await conncetion.ExecuteAsync("CategoryProcedure", parameter, commandType: System.Data.CommandType.StoredProcedure);
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
            using (var connection = new SqlConnection(_connectionString))
            {
                var getCategories = await connection.QueryAsync<CategoryDto>("GetAllCategory", commandType: CommandType.StoredProcedure);

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var updateCategory = await connection.QueryFirstOrDefaultAsync<CategoryDto>("UpdateCategory", parameter, commandType: CommandType.StoredProcedure);
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
        public async Task<ApiResponse<List<SearchByCategoryDto>>> SearchbyCategory(string searchcategory)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@categoryname", searchcategory);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cProducts = await connection.QueryAsync<SearchByCategoryDto>("SearchCategory", parameter, commandType: CommandType.StoredProcedure);
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
