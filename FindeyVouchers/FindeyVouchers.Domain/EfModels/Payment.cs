using System;

namespace FindeyVouchers.Domain.EfModels
{
    public class Payment
    {
        public Guid Id { get; set; }
        public float Amount { get; set; }
        public string StripeId { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
    }
}