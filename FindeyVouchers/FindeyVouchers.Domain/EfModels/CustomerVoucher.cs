using System;

namespace FindeyVouchers.Domain.EfModels
{
    public class CustomerVoucher
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime PurchasedOn { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
        public bool EmailSent { get; set; }
        public DateTime PurchasedDate { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}