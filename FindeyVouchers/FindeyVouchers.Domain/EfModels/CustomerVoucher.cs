using System;
using System.ComponentModel.DataAnnotations;

namespace FindeyVouchers.Domain.EfModels
{
    public class CustomerVoucher
    {
        public CustomerVoucher()
        {
            this.ValidUntil = DateTime.Now.AddYears(1);
            this.IsUsed = false;
        }

        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public MerchantVoucher VoucherMerchant { get; set; }

        [Display(Name = "Gekocht op")]
        [DataType(DataType.Date)]
        public DateTime PurchasedOn { get; set; }

        [Display(Name = "Prijs")] public decimal Price { get; set; }
        [Display(Name = "Voucher code")] public string Code { get; set; }

        [Display(Name = "Voucher verstuurd")] public bool EmailSent { get; set; }

        [Display(Name = "Geldig tot")]
        [DataType(DataType.Date)]
        public DateTime ValidUntil { get; set; }
        
        [Display(Name = "Voucher geldig?")]
        public bool IsUsed { get; set; }
    }
}