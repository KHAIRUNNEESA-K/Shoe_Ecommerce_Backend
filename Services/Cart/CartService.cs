using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Cart;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services.Cart
{
    public class CartService :ICartService
    {
        private readonly string _connectionString;
        public CartService(IConfiguration configuration)
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
            parameter.Add("@Flag", 600);

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var addcart = await connection.QueryAsync<CartItemDto>("SP_CartManagement", parameter, commandType: CommandType.StoredProcedure);
                    var addToCart = addcart.ToList();
                    if (!addToCart.Any())
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success = false,
                            Message="Product successfully added or updated in cart",
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
            parameter.Add("@Flag", 601);
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryAsync<CartItemDto>("SP_CartManagement", parameter, commandType: CommandType.StoredProcedure);
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
        public async Task<ApiResponse<List<CartItemDto>>> UpdateCartItem(int userId, int productId,int quantity)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@userId", userId);
                parameter.Add("@productId",productId);
                parameter.Add("@quantity", quantity);
                parameter.Add("@Flag", 602);

                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var executeUpdateCart=await connection.QueryAsync<CartItemDto>("SP_CartManagement",parameter,commandType:CommandType.StoredProcedure);
                    var updateCart = executeUpdateCart.ToList();
                    if (!updateCart.Any())
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success = false,
                            Message="Product successfully added or updated in cart",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<CartItemDto>>
                        {
                            Success = true,
                            Message="The Product successfully added to cart",
                            Data=updateCart

                        };
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public async Task<ApiResponse<DeleteCartDto>> DeleteCart(int userId, int productId)
        {
            var parameter= new DynamicParameters();
            parameter.Add("@userId", userId);
            parameter.Add("@productId",productId);
            parameter.Add("@Flag", 603);
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var executeDeleteCart = await connection.QueryFirstOrDefaultAsync<DeleteCartDto>("SP_CartManagement", parameter, commandType: CommandType.StoredProcedure);
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
