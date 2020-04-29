using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FindeyVouchers.Domain.EfModels
{
    public class ApplicationUser : IdentityUser
    {
        public string StripeAccountId { get; set; }
        public string Country { get; set; }
        public BusinessType BusinessType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)] public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        
    }

    public enum BusinessType
    {
        SoleProp,
        Corporation,
        NonProfit,
        Partnership,
        Llc
    }
}