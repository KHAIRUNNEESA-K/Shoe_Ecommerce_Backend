using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services
{
    public class ProductServices : IProductServices
    {
        private readonly string _connectionString;

        public ProductServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ApiResponse<ProductDto>> AddProduct(ProductDto addProduct)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var existingProduct = await connection.QueryFirstOrDefaultAsync<ProductDto>(
                "SELECT * FROM Products WHERE Name = @Name AND CategoryById = @CategoryById AND IsDeleted = 0",
                new { Name = addProduct.Name, CategoryById = addProduct.CategoryById });

                    if (existingProduct != null)
                    {
                        return new ApiResponse<ProductDto>
                        {
                            Success = false,
                            Message = "Product already added",
                            Data = null
                        };
                    }
                    var parameter = new DynamicParameters();
                    parameter.Add("@name", addProduct.Name);
                    parameter.Add("@descrption", addProduct.Description);
                    parameter.Add("@price", addProduct.Price);
                    parameter.Add("@size",addProduct.Size);
                    parameter.Add("@quatity", addProduct.Quantity);
                    parameter.Add("@imageurl", addProduct.Image_Url);
                    parameter.Add("@categorybyId", addProduct.CategoryById);
                    parameter.Add("@isactive", addProduct.IsActive);
                    parameter.Add("@isdeleted", false);
                    parameter.Add("@createdat", DateTime.Now);
                    parameter.Add("@updatedat", (DateTime?)null);
                    parameter.Add("@deletedAt", (DateTime?)null);

                    await connection.ExecuteAsync("AddProducts", parameter, commandType: CommandType.StoredProcedure);



                    return new ApiResponse<ProductDto>
                    {
                        Success = true,
                        Message="Product added successfully",
                        Data=addProduct
                    };


                }
                catch (Exception ex)
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success= false,
                        Message=$"Failed to add Product :{ex.Message}",
                        Data=null
                    };
                }
            }
        }
        public async Task<ApiResponse<List<ProductDto>>> GetAllProduct()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var response = await connection.QueryAsync<ProductDto>("GetAllProducts", commandType: CommandType.StoredProcedure);
                return new ApiResponse<List<ProductDto>>
                {
                    Success= true,
                    Message="All Products",
                    Data=response.ToList()
                };
            }
        }
        public async Task<ApiResponse<ProductDto>> GetProductById(int id)
        {
            var product = new DynamicParameters();
            product.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                var productbyid = await connection.QueryFirstOrDefaultAsync<ProductDto>("GetProductById", product, commandType: CommandType.StoredProcedure);
                if (productbyid==null)
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success= false,
                        Message="Product not found",
                        Data=null
                    };
                }
                else
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success= true,
                        Message="Product Details",
                        Data=productbyid
                    };
                }

            }
        }

    }
}
