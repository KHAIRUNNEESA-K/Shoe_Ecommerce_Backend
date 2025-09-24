using ONSTEPS_API.DTO.Address;
using ONSTEPS_API.Response;

namespace ONSTEPS_API.Services.Address
{
    public interface IAddress
    {
        Task<ApiResponse<AddressDto>> AddAddress (int userId,AddressRequestDto address);
        Task<ApiResponse<List<AddressDto>>> GetAddress(int userId);
        Task<ApiResponse<AddressDto>> GetAddressById (int userId,int Addressid);
        Task<ApiResponse<AddressDto>> UpdateAddress (int userId,int Addressid,AddressRequestDto address);
        Task<ApiResponse<AddressRequestDto>> DeleteAddress(int userId,int Addressid);
    }
}
