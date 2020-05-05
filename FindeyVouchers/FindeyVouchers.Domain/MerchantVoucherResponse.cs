using System;
using System.Collections.Generic;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Domain
{
    public class MerchantVoucherResponse
    {
        public List<Voucher> Vouchers { get; set; }
        public Merchant Merchant { get; set; }
    }

    public class Merchant
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Voucher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public VoucherType VoucherType { get; set; }
    }

    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}