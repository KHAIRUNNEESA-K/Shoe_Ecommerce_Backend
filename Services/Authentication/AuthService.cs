using BCrypt.Net;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ONSTEPS_API.DTO.Auth;
using ONSTEPS_API.JWT_Token;
using ONSTEPS_API.Models;
using ONSTEPS_API.Response;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ONSTEPS_API.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly string _connectionstring;
        private readonly AppDbContext _appDbContext;
        private readonly Token _token;

        public AuthService(AppDbContext dbContext, IConfiguration configuration, Token token)
        {
            _connectionstring=configuration.GetConnectionString("DefaultConnection");
            _appDbContext=dbContext;
            _token=token;
        }
        public async Task<ApiResponse<SignupDto>> Register(SignupDto userSignup)
        {

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userSignup.Password);

            using (var connection = new SqlConnection(_connectionstring))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Flag", 101);
                parameter.Add("@name", userSignup.Name);
                parameter.Add("@email", userSignup.Email);
                parameter.Add("@phone", userSignup.PhoneNo);
                parameter.Add("@username", userSignup.UserName);
                parameter.Add("@password", hashedPassword);
                parameter.Add("@role", "User");
                parameter.Add("@createdAt", DateTime.Now);
                parameter.Add("@updatedAt", DateTime.Now);
                parameter.Add("@deletedAt", DBNull.Value, DbType.DateTime);
                parameter.Add("@isblocked", false, DbType.Boolean);
                parameter.Add("@isdeleted", false, DbType.Boolean);
                

                try
                {
                    bool exist = await _appDbContext.Users.AnyAsync(u => u.UserName==userSignup.UserName|| u.Email== userSignup.Email || u.PhoneNo==userSignup.PhoneNo);
                    if (exist)
                    {
                        return new ApiResponse<SignupDto>
                        {
                            Success = false,
                            Message="user already exist",
                            Data=null
                        };
                    }
                    await connection.ExecuteAsync("SP_AuthManagement", parameter, commandType: CommandType.StoredProcedure);
                    var _userSignup = new SignupDto
                    {
                        Name= userSignup.UserName,
                        Email= userSignup.Email,
                        PhoneNo= userSignup.PhoneNo,
                        UserName= userSignup.UserName,

                    };

                    return new ApiResponse<SignupDto>
                    {
                        Success = true,
                        Message="Registration Successfull",
                        Data =_userSignup
                    };
                }
                catch (Exception ex)
                {
                    {
                        return new ApiResponse<SignupDto>
                        {
                            Success = false,
                            Message=$"Registration Failed : {ex.Message}",
                            Data=null
                        };
                    }

                }


            }
        }

        public async Task<ApiResponse<LoginResponseDto>> Login(LoginDto userLogin)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Flag", 100);
                parameter.Add("@username", userLogin.UserName);

                try
                {
                    var user = await con.QueryFirstOrDefaultAsync<User>("SP_AuthManagement", parameter, commandType: CommandType.StoredProcedure);

                    if (user==null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
                    {
                        return new ApiResponse<LoginResponseDto>
                        {
                            Success=false,
                            Message="Invalid Username or Password",
                            Data=null
                        };
                    }

                    if (user==null ||string.IsNullOrEmpty(userLogin.UserName)||string.IsNullOrEmpty(userLogin.Password))
                    {
                        return new ApiResponse<LoginResponseDto>
                        {
                            Success = false,
                            Message="Username and Password are required",
                            Data=null
                        };
                    }

                    var token = _token.GenerateJwtToken(user);

                    var userDto = new LoginResponseDto
                    {
                        UserId = user.UserId,
                        UserName=user.UserName,
                        Email=user.Email,
                        Token=token

                    };
                    string message = user.Role=="Admin" ? "Admin login Successful" : "User login Successful";
                    return new ApiResponse<LoginResponseDto>
                    {
                        Success=true,
                        Message=message,
                        Data=userDto
                    };


                }
                catch (Exception ex)
                {
                    return new ApiResponse<LoginResponseDto>
                    {
                        Success=false,
                        Message=$"Login Failed : {ex.Message}",
                        Data=null
                    };
                }

            }
        }
    }
}
