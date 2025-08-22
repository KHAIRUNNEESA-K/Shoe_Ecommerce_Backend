using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services
{
    public class CartServices :ICart
    {
        private readonly string _connectionString;
        public CartServices(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ApiResponse<List<CartItemDto>>> AddToCart(int userId,int productId,int quantity)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", userId);
            parameter.Add("@productId",productId);
            parameter.Add("@quantity",quantity);
            parameter.Add("@isDeleted",false);

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var addcart = await connection.QueryAsync<CartItemDto>("AddToCart", parameter, commandType: CommandType.StoredProcedure);
                    var addToCart = addcart.ToList();
                    if (!addToCart.Any())
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success = false,
                            Message="The product is already add to cart",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success = true,
                            Message="The Product successfully added to cart",
                            Data=addToCart

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CartItemDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data=null
                };
            }
        }




        public async Task<ApiResponse<List<CartItemDto>>> GetCartItems(int userId)
        {
            var parameter= new DynamicParameters();
            parameter.Add("@userId",userId);
            parameter.Add("@Flag",100)
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryAsync<CartItemDto>("GetCartItems", parameter, commandType: CommandType.StoredProcedure);
                    var getCart = response.ToList();
                    if (!getCart.Any())
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success = false,
                            Message="cart is empty",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success=true,
                            Message="Cart items",
                            Data=getCart
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CartItemDto>>
                {
                    Success=false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }
        public async Task<ApiResponse<DeleteCartDto>> DeleteCart(int userId, int productId)
        {
            var parameter= new DynamicParameters();
            parameter.Add("@userId", userId);
            parameter.Add("@productId",productId);
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var executeDeleteCart = await connection.QueryFirstOrDefaultAsync<DeleteCartDto>("DeleteCart", parameter, commandType: CommandType.StoredProcedure);
                    if (executeDeleteCart!=null)
                    {
                        return new ApiResponse<DeleteCartDto>                        {
                            Success = true,
                            Message="Successfully Delete Cart Item",
                            Data=executeDeleteCart
                        };
                    }
                    else
                    {
                        return new ApiResponse<DeleteCartDto>
                        {
                            Success = false,
                            Message="Error",
                            Data=null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteCartDto>
                {
                    Success=false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }
    }
}
