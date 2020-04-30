using System;
using System.ComponentModel.DataAnnotations;

namespace FindeyVouchers.Domain.EfModels
{
    public class CustomerVoucher
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        [Display(Name = "Gekocht op")] public DateTime PurchasedOn { get; set; }
        [Display(Name = "Prijs")] public decimal Price { get; set; }
        [Display(Name = "Voucher code")] public string Code { get; set; }

        [Display(Name = "Voucher verstuurd")] public bool EmailSent { get; set; }

        [Display(Name = "Geldig tot")] public DateTime ValidUntil { get; set; }
    }
}