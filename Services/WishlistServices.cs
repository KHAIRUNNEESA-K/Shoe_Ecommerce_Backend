using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;

namespace ONSTEPS_API.Services
{
    public class WishlistServices : IWishlist
    {
        private readonly string _connectionString;
        public WishlistServices(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ApiResponse<List<ProductDto>>> AddWishlist(int userId, int productId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", userId);
            parameter.Add("@productId", productId);
            parameter.Add("@isDeleted", false);
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var wishlist = await connection.QueryAsync<ProductDto>("AddWishlist", parameter, commandType: CommandType.StoredProcedure);
                    var wishList = wishlist.ToList();
                    if (!wishList.Any())
                    {
                        return new ApiResponse<List<ProductDto>>
                        {
                            Success = false,
                            Message="This product is already in wishlist",
                            Data=null
                        };
                    }
                    return new ApiResponse<List<ProductDto>>
                    {
                        Success = true,
                        Message="Product addd to wishlist",
                        Data=wishList

                    };

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }
        public async Task<ApiResponse<List<ProductDto>>> GetWishlist(int userId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", userId);
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryAsync<ProductDto>("GetWishlist", parameter, commandType: CommandType.StoredProcedure);
                    var wishlist = response.ToList();
                    if (!wishlist.Any())
                    {
                        return new ApiResponse<List<ProductDto>>
                        {
                            Success = false,
                            Message="Wishlist is Empty",
                            Data=null
                        };

                    }
                    else
                    {
                        return new ApiResponse<List<ProductDto>>
                        {
                            Success = true,
                            Message="Wishlist Products",
                            Data=wishlist
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }
        public async Task<ApiResponse<DeleteWishlistDto>> DeleteWishlist(int userId, int productId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", userId);
            parameter.Add("@productId", productId);

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var deleteWishlist = await connection.QueryFirstOrDefaultAsync<DeleteWishlistDto>("DeleteWishlist", parameter, commandType: CommandType.StoredProcedure);
                    if (deleteWishlist != null)
                    {
                        return new ApiResponse<DeleteWishlistDto>
                        {
                            Success = true,
                            Message="Product successfully remove from Wishlist",
                            Data=deleteWishlist
                        };
                    }
                    else
                    {
                        return new ApiResponse<DeleteWishlistDto>
                        {
                            Success=false,
                            Message="Error",
                            Data=null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteWishlistDto>
                {
                    Success = false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }
    }
}

