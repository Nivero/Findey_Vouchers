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

        [Display(Name = "Vestigingsadres van uw bedrijf")] public string Address { get; set; }

        [Display(Name = "Postcode")]
        [RegularExpression(@"^\d{4}?[aA-zZ]{2}$", ErrorMessage = "Ongeldige postcode")]
        public string ZipCode { get; set; }

        [Display(Name = "Stad")] public string City { get; set; }

        [Display(Name = "Land")] public string Country { get; set; } = "NL";

        [Display(Name = "Bedrijfsnaam")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$",
            ErrorMessage = "Bedrijfsnaam mag alleen alfanumerieke tekens bevatten.")]
        public string CompanyName { get; set; }

        public string NormalizedCompanyName { get; set; }

        [Display(Name = "Rechtsvorm bedrijf")] public BusinessType BusinessType { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^06?\d{8}$", ErrorMessage = "Gebruik 06 en alleen getallen.")]
        [Display(Name = "Telefoonnummer")]
        public override string PhoneNumber { get; set; }

        [Display(Name = "Bedrijfswebsite")] 
        public string Website { get; set; }

        [Display(Name = "Omschrijving bedrijf")]
        public string Description { get; set; }
    }

    public enum BusinessType
    {
        [Display(Name = "Eenmanszaak")] SoleProp,
        [Display(Name = "Overig")] Corporation
    }
}