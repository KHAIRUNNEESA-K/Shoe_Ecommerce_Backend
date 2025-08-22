using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ONSTEPS_API.DTO;
using ONSTEPS_API.Models;
using ONSTEPS_API.Response;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace ONSTEPS_API.Services
{
    public class AdminServices : IAdminServices
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
            using (var connect = new SqlConnection(_connection))
            {

                var response = await connect.QueryAsync<AdminResponseDto>("GetAllUsers", commandType: CommandType.StoredProcedure);
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
            parameters.Add("@id", id);

            using (var connect = new SqlConnection(_connection))
            {
                var userById = await connect.QueryFirstOrDefaultAsync<AdminResponseDto>("GetById", parameters, commandType: CommandType.StoredProcedure);
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
        public async Task<ApiResponse<HandleUserDto>> BlockUser(int id)
        {

            var Params = new DynamicParameters();
            Params.Add("@userId", id);

            

            using (var connect = new SqlConnection(_connection))
            {
                var blockuser = await connect.QueryFirstOrDefaultAsync<HandleUserDto>("BlockUser", Params, commandType: CommandType.StoredProcedure);



                if (blockuser == null)
                {
                    return new ApiResponse<HandleUserDto>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }
                else if (blockuser.Status == "Already blocked")
                {
                    return new ApiResponse<HandleUserDto>
                    {
                        Success = false,
                        Message ="The user is already blocked.",
                        Data = blockuser
                    };
                }
                else
                {
                    return new ApiResponse<HandleUserDto>
                    {
                        Success = true,
                        Message="The user has been successfully blocked.",
                        Data = blockuser
                    };
                }


            }
        }
        public async Task<ApiResponse<HandleUserDto>> UnBlockUser(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", id);
            using (var connect = new SqlConnection(_connection))
            {
                

                var unblockuser = await connect.QueryFirstOrDefaultAsync<HandleUserDto>("unBlockUser", parameter, commandType: CommandType.StoredProcedure);
                if (unblockuser==null)
                {
                    return new ApiResponse<HandleUserDto>
                    {
                        Success = false,
                        Message="User not found",
                        Data=null
                    };
                }

                else if (unblockuser.Status=="not blocked")
                {
                    return new ApiResponse<HandleUserDto>
                    {
                        Success=false,
                        Message="The user is not currently blocked.",
                        Data=unblockuser
                    };
                }
                
                else
                {
                    return new ApiResponse<HandleUserDto>
                    {
                        Success=true,
                        Message="The user has been successfully unblocked.",
                        Data=unblockuser
                    };
                }
            }
        }
    }
}
