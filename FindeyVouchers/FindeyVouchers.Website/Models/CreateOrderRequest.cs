using System;
using System.Collections.Generic;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Website.Models
{
    public class CreateOrderRequest
    {
        public Customer Customer { get; set; }
        public string PaymentId { get; set; }
        public List<Voucher> Vouchers { get; set; }
    }
}