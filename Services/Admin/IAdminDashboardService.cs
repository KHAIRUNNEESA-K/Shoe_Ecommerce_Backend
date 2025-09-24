using ONSTEPS_API.DTO.Admin;
using ONSTEPS_API.DTO.Revenue;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Admin
{
    public interface IAdminDashboardService
    {
        Task<ApiResponse<List<AdminResponseDto>>> GetAllUsers();
        Task<ApiResponse<AdminResponseDto>>GetById(int id);
        Task<ApiResponse<UserDto>> BlockUser(int id);
        Task<ApiResponse<UserDto>> UnBlockUser(int id);
        Task<ApiResponse<RevenueSummaryDto>> TotalRevenue();
        Task<ApiResponse<List<RevenueByDateDto>>> TotalRevenueByDate();
    }
}
