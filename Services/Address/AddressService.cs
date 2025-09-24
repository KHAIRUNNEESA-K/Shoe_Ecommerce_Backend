using Dapper;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Address;
using ONSTEPS_API.DTO.Product;
using ONSTEPS_API.Response;
using System.Data;

namespace ONSTEPS_API.Services.Address
{
    public class AddressService : IAddress
    {
        private readonly string _connectionString;

        public AddressService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ApiResponse<AddressDto>> AddAddress(int userId, AddressRequestDto address)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@UserId", userId);
                    parameter.Add("@FullName", address.FullName);
                    parameter.Add("@PhoneNumber", address.PhoneNumber);
                    parameter.Add("@Pincode", address.Pincode);
                    parameter.Add("@HouseNo", address.HouseNo);
                    parameter.Add("@Area", address.Area);
                    parameter.Add("@Landmark", address.Landmark);
                    parameter.Add("@City", address.City);
                    parameter.Add("@State", address.State);
                    parameter.Add("@Country", address.Country);
                    parameter.Add("@IsDefault", address.IsDefault);
                    parameter.Add("@Flag", 700);

                    // Capture the new AddressId returned by SCOPE_IDENTITY()
                    var newAddressId = await connection.QuerySingleAsync<int>(
                        "SP_AddressManagement",
                        parameter,
                        commandType: CommandType.StoredProcedure
                    );

                    if (newAddressId > 0)
                    {
                        var responseDto = new AddressDto
                        {
                            Address_Id = newAddressId,
                            UserId = userId,
                            FullName = address.FullName,
                            PhoneNumber = address.PhoneNumber,
                            Pincode = address.Pincode,
                            HouseNo = address.HouseNo,
                            Area = address.Area,
                            Landmark = address.Landmark,
                            City = address.City,
                            State = address.State,
                            Country = address.Country,
                            IsDefault = address.IsDefault
                        };

                        return new ApiResponse<AddressDto>
                        {
                            Success = true,
                            Message = "Address added successfully",
                            Data = responseDto
                        };
                    }
                    else
                    {
                        return new ApiResponse<AddressDto>
                        {
                            Success = false,
                            Message = "address already exist",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressDto>
                {
                    Success = false,
                    Message = $"Failed to add address: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<AddressDto>>> GetAddress(int userId)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@userId", userId);
                parameter.Add("@Flag", 701);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryAsync<AddressDto>("SP_AddressManagement", parameter, commandType: CommandType.StoredProcedure);
                    var getAddress = response.ToList();
                    if (!getAddress.Any())
                    {
                        return new ApiResponse<List<AddressDto>>()
                        {
                            Success= false,
                            Message="Address not found",
                            Data=null
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<AddressDto>>()
                        {
                            Success = false,
                            Message="Address Details",
                            Data=getAddress
                        };
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ApiResponse<AddressDto>> GetAddressById(int userId, int addressId)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Address_Id", addressId);
                parameter.Add("@UserId", userId);
                parameter.Add("@Flag", 702);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var response = await connection.QueryFirstOrDefaultAsync<AddressDto>(
                        "SP_AddressManagement",
                        parameter,
                        commandType: CommandType.StoredProcedure
                    );

                    if (response == null)
                    {
                        return new ApiResponse<AddressDto>
                        {
                            Success = false,
                            Message = "Address not found",
                            Data = null
                        };
                    }

                    return new ApiResponse<AddressDto>
                    {
                        Success = true,
                        Message = "Address details fetched successfully",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressDto>
                {
                    Success = false,
                    Message = $"Error fetching address: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<AddressDto>> UpdateAddress(int userId, int Addressid, AddressRequestDto addressDto)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@userId", userId);
                parameter.Add("@Address_id", Addressid);
                if (addressDto.FullName != null) parameter.Add("@FullName", addressDto.FullName);
                if (addressDto.PhoneNumber != null) parameter.Add("@PhoneNumber", addressDto.PhoneNumber);
                if (addressDto.Pincode !=null) parameter.Add("@Pincode", addressDto.Pincode);
                if (addressDto.HouseNo != null) parameter.Add("@HouseNo", addressDto.HouseNo);
                if (addressDto.Area !=null) parameter.Add("@Area", addressDto.Area);
                if (addressDto.Landmark !=null) parameter.Add("@Landmark", addressDto.Landmark);
                if (addressDto.City != null) parameter.Add("@City", addressDto.City);
                if (addressDto.State !=null) parameter.Add("@State", addressDto.State);
                if (addressDto.Country != null) parameter.Add("@Country", addressDto.Country);

                parameter.Add("@IsDefault", addressDto.IsDefault);
                parameter.Add("@Flag", 703);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var updateAddress = await connection.QueryFirstOrDefaultAsync<AddressDto>(
                        "SP_AddressManagement", parameter, commandType: CommandType.StoredProcedure);

                    return updateAddress == null
                        ? new ApiResponse<AddressDto>
                        {
                            Success = false,
                            Message = "Address not found",
                            Data = null
                        }
                        : new ApiResponse<AddressDto>
                        {
                            Success = true,
                            Message = "Address Updated Successfully",
                            Data = updateAddress
                        };
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<ApiResponse<AddressRequestDto>> DeleteAddress(int userId,int Addressid)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", userId);
            parameter.Add("@Address_Id", Addressid);
            parameter.Add("@Flag", 704);

            using (var connection = new SqlConnection(_connectionString))
            {
                var deleteAddress = await connection.QueryFirstOrDefaultAsync<AddressRequestDto>(
                    "SP_AddressManagement", parameter, commandType: CommandType.StoredProcedure);

                if (deleteAddress == null)
                {
                    return new ApiResponse<AddressRequestDto>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = null
                    };
                }

                return new ApiResponse<AddressRequestDto>
                {
                    Success = true,
                    Message = "Address Deleted Successfully",
                    Data = deleteAddress
                };
            }
        }

    }
}




