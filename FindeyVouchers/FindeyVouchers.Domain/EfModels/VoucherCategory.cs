using System;
using System.ComponentModel.DataAnnotations;

namespace FindeyVouchers.Domain.EfModels
{
    public class VoucherCategory
    {
        public Guid Id { get; set; }

        [Display(Name = "Categorie naam")] public string Name { get; set; }

        [Display(Name = "Positie")] public int Ranking { get; set; }

        public ApplicationUser Merchant { get; set; }
    }
}