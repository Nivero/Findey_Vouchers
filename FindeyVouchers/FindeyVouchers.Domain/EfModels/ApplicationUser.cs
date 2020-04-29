using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FindeyVouchers.Domain.EfModels
{
    public class ApplicationUser : IdentityUser
    {
        public string StripeAccountId { get; set; }

        [Display(Name = "Voornaam")] public string FirstName { get; set; }
        [Display(Name = "Achternaam")] public string LastName { get; set; }

        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Adres")] public string Address { get; set; }

        [Display(Name = "Postcode")]
        [RegularExpression(@"^\d{4} ?[aA-zZ]{2}$", ErrorMessage = "Ongeldige postcode")]
        public string ZipCode { get; set; }

        [Display(Name = "Stad")] public string City { get; set; }
        [Display(Name = "Land")] public string Country { get; set; }
        [Display(Name = "Rechtsvorm bedrijf")] public BusinessType BusinessType { get; set; }
        [DataType(DataType.PhoneNumber)] public override string PhoneNumber { get; set; }
    }

    public enum BusinessType
    {
        [Display(Name = "Eenmanszaak")] SoleProp,
        [Display(Name = "Bedrijf")] Corporation
    }
}