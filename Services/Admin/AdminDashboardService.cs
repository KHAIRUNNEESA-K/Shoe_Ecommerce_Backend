using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ONSTEPS_API.DTO.Admin;
using ONSTEPS_API.DTO.Revenue;
using ONSTEPS_API.Models;
using ONSTEPS_API.Response;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;

namespace ONSTEPS_API.Services.Admin
{
    public class AdminServices : IAdminDashboardService
    {
        private readonly string _connection;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public AdminServices(AppDbContext appDbContext,IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DefaultConnection");

        }
        public async Task<ApiResponse<List<AdminResponseDto>>> GetAllUsers()
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Flag", 200);
            using (var connect = new SqlConnection(_connection))
            {

                var response = await connect.QueryAsync<AdminResponseDto>("SP_UserManagement", parameter, commandType: CommandType.StoredProcedure);
                return new ApiResponse<List<AdminResponseDto>>
                {
                    Success= true,
                    Message="User Details",
                    Data=response.ToList()
                };

            }
        }
        public async Task<ApiResponse<AdminResponseDto>> GetById(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@userId", id);
            parameters.Add("@Flag", 201);

            using (var connect = new SqlConnection(_connection))
            {
                var userById = await connect.QueryFirstOrDefaultAsync<AdminResponseDto>("SP_UserManagement", parameters, commandType: CommandType.StoredProcedure);
                if (userById == null)
                {
                    return new ApiResponse<AdminResponseDto>
                    {
                        Success= false,
                        Message="User not found",
                        Data=null
                    };
                }
                else
                {
                    return new ApiResponse<AdminResponseDto>
                    {
                        Success = true,
                        Message="User details",
                        Data=userById
                    };
                }

            }
        }
        public async Task<ApiResponse<UserDto>> BlockUser(int id)
        {
            try
            {
                var Parameter = new DynamicParameters();
                Parameter.Add("@userId", id);
                Parameter.Add("@Flag", 202); 

                using (var connect = new SqlConnection(_connection))
                {
                    var blockuser = await connect.QueryFirstOrDefaultAsync<UserDto>("SP_UserManagement", Parameter, commandType: CommandType.StoredProcedure);
                      
                    if (blockuser == null)
                    {
                        return new ApiResponse<UserDto>
                        {
                            Success = false,
                            Message = "User not found",
                            Data = null
                        };
                    }
                    else if (blockuser.Status == "Already blocked")
                    {
                        return new ApiResponse<UserDto>
                        {
                            Success = false,
                            Message ="The user is already blocked.",
                            Data = blockuser
                        };
                    }
                    else
                    {
                        return new ApiResponse<UserDto>
                        {
                            Success = true,
                            Message="The user has been successfully blocked.",
                            Data = blockuser
                        };
                    } 
                }
            }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
        public async Task<ApiResponse<UserDto>> UnBlockUser(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", id);
            parameter.Add("@Flag", 203);
            using (var connect = new SqlConnection(_connection))
            {
                

                var unblockuser = await connect.QueryFirstOrDefaultAsync<UserDto>("SP_UserManagement", parameter, commandType: CommandType.StoredProcedure);
                if (unblockuser==null)
                {
                    return new ApiResponse<UserDto>
                    {
                        Success = false,
                        Message="User not found",
                        Data=null
                    };
                }

                else if (unblockuser.Status=="not blocked")
                {
                    return new ApiResponse<UserDto>
                    {
                        Success=false,
                        Message="The user is not currently blocked.",
                        Data=unblockuser
                    };
                }
                
                else
                {
                    return new ApiResponse<UserDto>
                    {
                        Success=true,
                        Message="The user has been successfully unblocked.",
                        Data=unblockuser
                    };
                }
            }
        }
        public async Task<ApiResponse<RevenueSummaryDto>> TotalRevenue()
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    var getRevenue = await connection.QueryFirstOrDefaultAsync<RevenueSummaryDto>("SP_TotalRevenue", commandType: CommandType.StoredProcedure);
                    if (getRevenue==null)
                    {
                        return new ApiResponse<RevenueSummaryDto>
                        {
                            Success=false,
                            Message="Revenue not uploaded",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<RevenueSummaryDto>
                        {
                            Success=true,
                            Message="Total revenue ",
                            Data=getRevenue
                        };
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ApiResponse<List<RevenueByDateDto>>> TotalRevenueByDate()
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    var revenueByDate = await connection.QueryAsync<RevenueByDateDto>(
                        "SP_RevenueByDate",
                        commandType: CommandType.StoredProcedure
                    );

                    var revenueList = revenueByDate.ToList();

                    if (!revenueList.Any())
                    {
                        return new ApiResponse<List<RevenueByDateDto>>
                        {
                            Success = false,
                            Message = "Failed to load Revenue By Date",
                            Data = null
                        };
                    }

                    return new ApiResponse<List<RevenueByDateDto>>
                    {
                        Success = true,
                        Message = "Revenue By Date",
                        Data = revenueList
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RevenueByDateDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

    }
}
