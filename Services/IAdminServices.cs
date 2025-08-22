using ONSTEPS_API.DTO;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services
{
    public interface IAdminServices
    {
        Task<ApiResponse<List<AdminResponseDto>>> GetAllUsers();
        Task<ApiResponse<AdminResponseDto>>GetById(int id);
        Task<ApiResponse<HandleUserDto>> BlockUser(int id);
        Task<ApiResponse<HandleUserDto>> UnBlockUser(int id);
    }
}
