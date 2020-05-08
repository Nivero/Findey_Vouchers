using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Website.Models
{
    public class PaymentSuccessRequest
    {
        public Customer Customer { get; set; }
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public int Amount { get; set; }
        public long Created { get; set; }
        
    }
}