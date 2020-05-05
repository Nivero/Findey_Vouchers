using System;
using System.Collections.Generic;
using System.Linq;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Models
{
    public class MerchantVoucherViewModel
    {
        public MerchantVoucher Voucher { get; set; }
        public List<VoucherCategory> Categories { get; set; }
        
        public Guid CategoryId { get; set; }
    }
}