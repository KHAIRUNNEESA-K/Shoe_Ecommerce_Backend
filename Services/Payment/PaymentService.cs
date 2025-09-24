using Dapper;

using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using ONSTEPS_API.DTO.Payment;
using ONSTEPS_API.Response;
using ONSTEPS_API.Services.Payment;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class PaymentService : IPaymentService
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;

    public PaymentService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _configuration = configuration;
    }

    public async Task<ApiResponse<PaymentResponseDto>> CreatePayment(int userId, int orderId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);


            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId);

            var payment = await connection.QueryFirstOrDefaultAsync<PaymentResponseDto>(
                "SP_CreatePayment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (payment == null)
            {
                return new ApiResponse<PaymentResponseDto>
                {
                    Success = false,
                    Message = "Failed to create payment",
                    Data = null
                };
            }


      //      var user = await connection.QueryFirstOrDefaultAsync<dynamic>(
      //           @"SELECT 
      //    Name AS UserName,       
      //    Email AS UserEmail
      //FROM Users 
      //WHERE User_Id = @UserId",
      //          new { UserId = userId }
      //      );

      //      payment.UserName = user?.UserName ?? "";
      //      payment.UserEmail = user?.UserEmail ?? "";
          

            var client = new Razorpay.Api.RazorpayClient(
                _configuration["Razorpay:KeyId"],
                _configuration["Razorpay:KeySecret"]
            );

            var options = new Dictionary<string, object>
        {
            { "amount", payment.Amount * 100 },
            { "currency", payment.Currency },
            { "receipt", orderId.ToString() },
            { "payment_capture", 1 }
        };

            var razorpayOrder = client.Order.Create(options);

         
            var updateParams = new DynamicParameters();
            updateParams.Add("@PaymentId", payment.PaymentId);
            updateParams.Add("@RazorpayOrderId", razorpayOrder["id"]);

            await connection.ExecuteAsync(
                "UPDATE Payments SET RazorpayOrderId = @RazorpayOrderId WHERE PaymentId = @PaymentId",
                updateParams
            );

            payment.RazorpayOrderId = razorpayOrder["id"].ToString();


            return new ApiResponse<PaymentResponseDto>
            {
                Success = true,
                Message = "Payment created successfully",
                Data = payment
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaymentResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }



    public async Task<ApiResponse<PaymentResponseDto>> VerifyPayment(int userId, VerifyPaymentDto dto)
    {
        try
        {
            // 1️⃣ Generate Razorpay signature
            string generatedSignature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_configuration["Razorpay:KeySecret"])))
            {
                var data = $"{dto.RazorpayOrderId}|{dto.RazorpayPaymentId}";
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                generatedSignature = Convert.ToHexString(hash).ToLower();
            }

            if (generatedSignature != dto.RazorpaySignature.ToLower())
            {
                return new ApiResponse<PaymentResponseDto>
                {
                    Success = false,
                    Message = "Invalid Razorpay Signature",
                    Data = null
                };
            }

            // 2️⃣ Update payment in DB
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", dto.OrderId);
            parameters.Add("@RazorpayPaymentId", dto.RazorpayPaymentId);
            parameters.Add("@RazorpayOrderId", dto.RazorpayOrderId);
            parameters.Add("@RazorpaySignature", dto.RazorpaySignature);
            parameters.Add("@Status", "Paid");

            var payment = await connection.QueryFirstOrDefaultAsync<PaymentResponseDto>(
                "SP_VerifyPayment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (payment == null)
            {
                return new ApiResponse<PaymentResponseDto>
                {
                    Success = false,
                    Message = "Failed to verify payment",
                    Data = null
                };
            }

            return new ApiResponse<PaymentResponseDto>
            {
                Success = true,
                Message = "Payment verified successfully",
                Data = payment
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaymentResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }


}


