namespace ONSTEPS_API.DTO.Payment
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string RazorpayOrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
    }
}
