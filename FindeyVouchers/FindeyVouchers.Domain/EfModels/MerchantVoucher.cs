using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace FindeyVouchers.Domain.EfModels
{
    public class MerchantVoucher
    {
        public MerchantVoucher()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsActive = true;
            this.AmountSold = 0;
            this.Image = "default-image.jpeg";
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

        [NotMapped]
        [AllowedExtensions(new string[] {".jpg", ".png", ".gif", ".jpeg"})]
        public IFormFile ImageFile { get; set; }

        [NotMapped] [Display(Name = "Aantal verkocht")] public int AmountSold { get; set; }
        [Display(Name = "Type voucher")] public VoucherType VoucherType { get; set; }
    }

    public enum VoucherType
    {
        [Display(Name = "Waardebon")] Voucher,
        [Display(Name = "Prepaid voucher")] PrepaidCard
    }


    internal class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _Extensions;

        public AllowedExtensionsAttribute(string[] Extensions)
        {
            _Extensions = Extensions;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!((IList) _Extensions).Contains(extension.ToLower()))
                {
                    return new ValidationResult($"Alleen .png .jpg of .gif is toegestaan.");
                }
            }

            return ValidationResult.Success;
        }
    }
}