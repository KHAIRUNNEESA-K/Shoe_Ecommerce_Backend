using ONSTEPS_API.DTO.Auth;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Authentication
{
    public interface IAuthService
    {
        Task<ApiResponse<SignupDto>> Register(SignupDto userSignup);
        Task<ApiResponse<LoginResponseDto>> Login(LoginDto userLogin);
    }
}
