using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface IAuthServices
    {
        Task<ApiResponse<SignupDto>> Register(SignupDto userSignup);
        Task<ApiResponse<LoginResponseDto>> Login(LoginDto userLogin);
    }
}
