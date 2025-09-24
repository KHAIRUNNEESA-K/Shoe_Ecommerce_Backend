namespace ONSTEPS_API.DTO.Payment
{
    public class PaymentCaptureRequestDto
    {
        public int OrderId { get; set; }
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string RazorpaySignature { get; set; }
    }
}
