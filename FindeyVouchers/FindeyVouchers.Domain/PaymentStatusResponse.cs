namespace FindeyVouchers.Domain
{
    public class PaymentStatusResponse
    {
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public long Amount { get; set; }
        public long Created { get; set; }
        public string ErrorMessage { get; set; }
    }
}