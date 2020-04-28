using Microsoft.AspNetCore.Identity;

namespace FindeyVouchers.Domain.EfModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }
        public int KvkNumber { get; set; }
        public string IbanNumber { get; set; }
        public LegalEntity LegalEntity { get; set; }
    }

    public enum LegalEntity
    {
        Individual,
        Business
    }
}