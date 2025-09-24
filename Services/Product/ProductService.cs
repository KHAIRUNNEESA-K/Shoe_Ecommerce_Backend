using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Product;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration configuration)
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
                "SELECT * FROM Products WHERE Product_Name = @Name AND CategoryById = @CategoryById AND IsDeleted = 0",
                new { Name = addProduct.Product_Name, addProduct.CategoryById });

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
                    parameter.Add("@Name", addProduct.Product_Name);
                    parameter.Add("@Description", addProduct.Description);
                    parameter.Add("@Price", addProduct.Price);
                    parameter.Add("@Size",addProduct.Size);
                    parameter.Add("@Quantity", addProduct.Quantity);
                    parameter.Add("@Image_Url", addProduct.Image_Url);
                    parameter.Add("@CategoryById", addProduct.CategoryById);
                    parameter.Add("@IsActive", addProduct.IsActive);
                    parameter.Add("@Flag", 300);

                    await connection.ExecuteAsync("SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure);



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
            var parameter=new DynamicParameters();
            parameter.Add("@Flag", 301);
            using (var connection = new SqlConnection(_connectionString))
            {
                var response = await connection.QueryAsync<ProductDto>("SP_ProductManagement", parameter,commandType: CommandType.StoredProcedure);
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
            var parameter = new DynamicParameters();
            parameter.Add("@ProductID", id);
            parameter.Add("@Flag", 302);


            using (var connection = new SqlConnection(_connectionString))
            {
                var productbyid = await connection.QueryFirstOrDefaultAsync<ProductDto>("SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure);
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
        public async Task<ApiResponse<ProductUpdateDto>> UpdateProducts(int id, ProductUpdateDto product)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ProductID", id);
            if (product.Product_Name != null) parameter.Add("@Name", product.Product_Name);
            if (product.Description != null) parameter.Add("@Description", product.Description);
            if (product.Price.HasValue) parameter.Add("@Price", product.Price);
            if (product.Size != null) parameter.Add("@Size", product.Size);
            if (product.Quantity.HasValue) parameter.Add("@Quantity", product.Quantity);
            if (product.Image_Url != null) parameter.Add("@Image_Url", product.Image_Url);
            if (product.CategoryById.HasValue) parameter.Add("@CategoryById", product.CategoryById);
            if (product.IsActive.HasValue) parameter.Add("@IsActive", product.IsActive);

            parameter.Add("@Flag", 303);

            using (var connection = new SqlConnection(_connectionString))
            {
                var updateProduct = await connection.QueryFirstOrDefaultAsync<ProductUpdateDto>(
                    "SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure);

                return updateProduct == null
                    ? new ApiResponse<ProductUpdateDto>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = null
                    }
                    : new ApiResponse<ProductUpdateDto>
                    {
                        Success = true,
                        Message = "Product Updated Successfully",
                        Data = updateProduct
                    };
            }
        }

        public async Task<ApiResponse<DeleteProductDto>> DeleteProduct(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ProductID", id);
            parameter.Add("@Flag", 304);

            using (var connection = new SqlConnection(_connectionString))
            {
                var deleteProduct = await connection.QueryFirstOrDefaultAsync<DeleteProductDto>(
                    "SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure);

                if (deleteProduct == null)
                {
                    return new ApiResponse<DeleteProductDto>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = null
                    };
                }

                return new ApiResponse<DeleteProductDto>
                {
                    Success = true,
                    Message = deleteProduct.Status == "Deleted" ? "Product Deleted Successfully" : deleteProduct.Status,
                    Data = deleteProduct
                };
            }
        }

       
        public async Task<ApiResponse<List<ProductDto>>> SearchProducts(string search)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@SearchTerm", search);
            parameter.Add("@Flag", 305);
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sproduct = await connection.QueryAsync<ProductDto>("SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure);
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
                parameter.Add("@PageNumber", pageNumber);
                parameter.Add("@PageSize", pageSize);
                parameter.Add("@Flag", 307);
                try
                {

                    using (var pagination = await connection.QueryMultipleAsync("SP_ProductManagement", parameter, commandType: CommandType.StoredProcedure))
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

