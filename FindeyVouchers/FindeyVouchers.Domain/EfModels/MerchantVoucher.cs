using System;
using System.ComponentModel.DataAnnotations;

namespace FindeyVouchers.Domain.EfModels
{
    public class MerchantVoucher
    {
        public MerchantVoucher()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsActive = true;
        }

        public Guid Id { get; set; }
        public ApplicationUser Merchant { get; set; }

        [Display(Name = "Naam van de voucher")]
        public string Name { get; set; }

        [Display(Name = "Beschrijving van de voucher")]
        public string Description { get; set; }

        [Display(Name = "Voucher afbeelding")] public string Image { get; set; }

        [Display(Name = "Actief")] public bool IsActive { get; set; }
        [Display(Name = "Aangemaakt op")]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Prijs")] public Decimal Price { get; set; }
    }
}