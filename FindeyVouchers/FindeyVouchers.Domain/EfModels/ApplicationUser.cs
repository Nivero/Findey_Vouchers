using Microsoft.AspNetCore.Identity;

namespace FindeyVouchers.Domain.EfModels
{
    public class ApplicationUser : IdentityUser
    {
        public string CompanyName { get; set; }
    }
}