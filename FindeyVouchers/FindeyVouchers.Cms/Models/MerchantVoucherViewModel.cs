using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Models
{
    public class MerchantVoucherViewModel
    {
        public MerchantVoucher Voucher { get; set; }
        public List<VoucherCategory> Categories { get; set; }

        [Display(Name = "Categorie")] public Guid CategoryId { get; set; }
    }
}