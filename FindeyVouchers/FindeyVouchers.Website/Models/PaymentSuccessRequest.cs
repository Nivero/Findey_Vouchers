using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Website.Models
{
    public class PaymentSuccessRequest
    {
        public Customer Customer { get; set; }
        public string PaymentId { get; set; }
    }
}