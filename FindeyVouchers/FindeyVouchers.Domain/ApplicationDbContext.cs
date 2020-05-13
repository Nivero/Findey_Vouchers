using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FindeyVouchers.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MerchantVoucher> MerchantVouchers { get; set; }
        public DbSet<StripeSecret> StripeSecret { get; set; }
        public DbSet<CustomerVoucher> CustomerVouchers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<VoucherCategory> VoucherCategories { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}