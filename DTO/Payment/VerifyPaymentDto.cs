﻿namespace ONSTEPS_API.DTO.Payment
{
    public class VerifyPaymentDto
    {
        public int OrderId { get; set; }
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string RazorpaySignature { get; set; }
        public string Status { get; set; }
    }
}
