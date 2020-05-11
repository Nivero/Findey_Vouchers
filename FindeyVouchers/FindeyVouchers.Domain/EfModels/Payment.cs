using System;

namespace FindeyVouchers.Domain.EfModels
{
    public class Payment
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}