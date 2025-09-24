using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Cart;
using ONSTEPS_API.DTO.Order;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services.Order
{
    public class OrderServices : IOrder
    {
        private readonly string _connectionString;
        public OrderServices(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ApiResponse<OrderDto>> PlaceOrder(int userId, OrderRequestDto orderDto)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userId);
                parameter.Add("@AddressId", orderDto.AddressId);
                parameter.Add("@Flag", 800);

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var multi = await connection.QueryMultipleAsync(
                        "SP_OrderManagement",
                        parameter,
                        commandType: CommandType.StoredProcedure))
                    {
                        var placeOrder = await multi.ReadFirstOrDefaultAsync<OrderDto>();

                        if (placeOrder == null)
                        {
                            return new ApiResponse<OrderDto>
                            {
                                Success = false,
                                Message = "you cart is empty",
                                Data = null
                            };
                        }

                        return new ApiResponse<OrderDto>
                        {
                            Success = true,
                            Message = "Placed order Successfully",
                            Data = placeOrder
                        };
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ApiResponse<List<OrderDto>>> GetOrders(int userId)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userId);
                using (var connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryAsync<OrderDto>("SP_GetOrdersByUser", parameter, commandType: CommandType.StoredProcedure);
                    var getOrder = response.ToList();
                    if (!getOrder.Any())
                    {
                        return new ApiResponse<List<OrderDto>>
                        {
                            Success = false,
                            Message="No order history",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<OrderDto>>
                        {
                            Success=true,
                            Message="order items",
                            Data=getOrder
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderDto>>
                {
                    Success=false,
                    Message=ex.Message,
                    Data=null
                };

            }
        }
        public async Task<ApiResponse<OrderDetailsDto>> GetOrderDetails(int userId, int orderId)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userId);
                parameter.Add("@OrderId", orderId);
                parameter.Add("@Flag", 802);

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var multi = await connection.QueryMultipleAsync(
                        "SP_OrderManagement",
                        parameter,
                        commandType: CommandType.StoredProcedure))
                    {
                        var order = await multi.ReadFirstOrDefaultAsync<OrderDto>();
                        var items = (await multi.ReadAsync<OrderItemDto>()).ToList();

                        if (order == null)
                        {
                            return new ApiResponse<OrderDetailsDto>
                            {
                                Success = false,
                                Message = "Order not found",
                                Data = null
                            };
                        }

                        var orderDetails = new OrderDetailsDto
                        {
                            Order = order,
                            Items = items
                        };

                        return new ApiResponse<OrderDetailsDto>
                        {
                            Success = true,
                            Message = "Order details fetched successfully",
                            Data = orderDetails
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderDetailsDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<OrderStatusDto>> UpdateOrderStatus(int userId,int orderId, string status)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@OrderId", orderId);
                parameter.Add("@Status", status);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryFirstOrDefaultAsync<OrderStatusDto>(
                        "SP_UpdateOrderStatus", parameter, commandType: CommandType.StoredProcedure);

                    return new ApiResponse<OrderStatusDto>
                    {
                        Success = true,
                        Message = "Order status updated",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderStatusDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
