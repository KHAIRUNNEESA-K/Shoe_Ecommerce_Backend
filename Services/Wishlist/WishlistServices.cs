using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Product;
using ONSTEPS_API.DTO.Wishlist;
using ONSTEPS_API.Response;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;

namespace ONSTEPS_API.Services.Wishlist
{
    public class WishlistServices : IWishlistService
    {
        private readonly string _connectionString;
        public WishlistServices(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ApiResponse<List<WishlistProductDto>>> AddWishlist(int userId, int productId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", userId);
            parameter.Add("@ProductId", productId);
            parameter.Add("@IsDeleted", false);
            parameter.Add("@Flag", 500);
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var wishlist = await connection.QueryAsync<WishlistProductDto>("SP_WishlistManagement", parameter, commandType: CommandType.StoredProcedure);
                    var wishList = wishlist.ToList();
                    if (!wishList.Any())
                    {
                        return new ApiResponse<List<WishlistProductDto>>
                        {
                            Success = false,
                            Message="This product is already in wishlist",
                            Data=null
                        };
                    }
                    return new ApiResponse<List<WishlistProductDto>>
                    {
                        Success = true,
                        Message="Product add to wishlist",
                        Data=wishList

                    };

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<WishlistProductDto>>
                {
                    Success = false,
                    Message=ex.Message,
                    Data=null
                };
            }
        }
        public async Task<ApiResponse<List<WishlistProductDto>>> GetWishlist(int userId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", userId);
            parameter.Add("@Flag", 501);
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryAsync<WishlistProductDto>("SP_WishlistManagement", parameter, commandType: CommandType.StoredProcedure);
                    var wishlist = response.ToList();
                    if (!wishlist.Any())
                    {
                        return new ApiResponse<List<WishlistProductDto>>
                        {
                            Success = false,
                            Message="Wishlist is Empty",
                            Data=null
                        };

                    }
                    else
                    {
                        return new ApiResponse<List<WishlistProductDto>>
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
                return new ApiResponse<List<WishlistProductDto>>
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
            parameter.Add("@UserId", userId);
            parameter.Add("@ProductId", productId);
            parameter.Add("@Flag", 502);

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var deleteWishlist = await connection.QueryFirstOrDefaultAsync<DeleteWishlistDto>("SP_WishlistManagement", parameter, commandType: CommandType.StoredProcedure);
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

