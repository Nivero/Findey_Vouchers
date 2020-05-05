using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FindeyVouchers.Domain.EfModels
{
    public class VoucherCategory
    {
        public Guid Id { get; set; }
        [Display(Name = "Categorie naam")]
        public string Name { get; set; }
        public ApplicationUser Merchant { get; set; }
    }
}