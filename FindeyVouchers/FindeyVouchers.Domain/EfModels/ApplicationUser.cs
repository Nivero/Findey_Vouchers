using System;
using Microsoft.AspNetCore.Identity;

namespace FindeyVouchers.Domain.EfModels
{
    public class ApplicationUser : IdentityUser
    {

    }

    public enum LegalEntity
    {
        Individual,
        Business
    }

    public enum Gender
    {
        Male,
        Female,
        Unknown
    }
}