using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services
{
    public class ProductHandleServices : IProducts
    {
        private readonly string _connectionString;
        public ProductHandleServices(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ApiResponse<DeleteProductDto>> DeleteProduct(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                var deleteproduct = await connection.QueryFirstOrDefaultAsync<DeleteProductDto>("DeleteProduct", parameter, commandType: CommandType.StoredProcedure);
                if (deleteproduct == null)
                {
                    return new ApiResponse<DeleteProductDto>
                    {
                        Success = false,
                        Message="Product not found",
                        Data=null
                    };
                }
                else if (deleteproduct.Status=="Already deleted")
                {
                    return new ApiResponse<DeleteProductDto>
                    {
                        Success = false,
                        Message="Product already Deleted",
                        Data=deleteproduct

                    };
                }
                else
                {
                    return new ApiResponse<DeleteProductDto>
                    {
                        Success = true,
                        Message="Product Deleted Successfully",
                        Data=deleteproduct
                    };
                }
            }
        }
        public async Task<ApiResponse<ProductUpdateDto>> UpdateProducts(int id, ProductUpdateDto product)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            if (product.Name != null) parameter.Add("@name", product.Name);
            if (product.Description != null) parameter.Add("@description", product.Description);
            if (product.Price != null) parameter.Add("@price", product.Price);
            if(product.Size !=null)parameter.Add("@size",product.Size);
            if (product.Quantity != null) parameter.Add("@quantity", product.Quantity);
            if (product.Image_Url != null) parameter.Add("@imageurl", product.Image_Url);
            if (product.CategoryById != null) parameter.Add("@categorybyId", product.CategoryById);
            if (product.IsActive != null) parameter.Add("@isactive", product.IsActive);

            parameter.Add("@isdeleted", false);
            parameter.Add("@updatedat", DateTime.Now);

            using (var connection = new SqlConnection(_connectionString))
            {
                var updateProduct = await connection.QueryFirstOrDefaultAsync<ProductUpdateDto>("UpdateProduct", parameter, commandType: CommandType.StoredProcedure);
                if (updateProduct==null)
                {
                    return new ApiResponse<ProductUpdateDto>
                    {
                        Success = false,
                        Message="Product not found",
                        Data=null
                    };
                }
                else
                {
                    return new ApiResponse<ProductUpdateDto>
                    {
                        Success=true,
                        Message="Product Update Successfully",
                        Data=updateProduct
                    };
                }
            }

        }
        public async Task<ApiResponse<List<ProductDto>>> SearchProducts(string search)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@searchTerms", search);
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sproduct = await connection.QueryAsync<ProductDto>("SearchProduct", parameter, commandType: CommandType.StoredProcedure);
                    var searchProduct = sproduct.ToList();
                    if (!searchProduct.Any())
                    {
                        return new ApiResponse<List<ProductDto>>
                        {
                            Success=false,
                            Message="Product not found",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<ProductDto>>
                        {
                            Success=true,
                            Message="Searched Product",
                            Data=searchProduct
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Message=$"An error occured {ex.Message}",
                    Data=null
                };
            }


        }
        public async Task<ApiResponse<PaginationDto<ProductDto>>> PaginationedProduct(int pageNumber, int pageSize)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                var parameter = new DynamicParameters();
                parameter.Add("@pageNumber", pageNumber);
                parameter.Add("@pageSize", pageSize);
                try
                {

                    using (var pagination = await connection.QueryMultipleAsync("Pagination", parameter, commandType: CommandType.StoredProcedure))
                    {
                        var products = (await pagination.ReadAsync<ProductDto>()).ToList();
                        var totalcount = (await pagination.ReadAsync<int>()).FirstOrDefault();

                        var result = new PaginationDto<ProductDto>
                        {
                            Data=products,
                            TotalCount=totalcount,
                            PageNumber=pageNumber,
                            PageSize=pageSize
                        };
                        return new ApiResponse<PaginationDto<ProductDto>>
                        {
                            Success = true,
                            Message="Pagination Successfull",
                            Data=result

                        };

                    }

                }
                catch (Exception ex)
                {
                    return new ApiResponse<PaginationDto<ProductDto>>
                    {
                        Success = false,
                        Message=ex.Message,
                        Data=null
                    };
                }
            }
        }

    }

}
