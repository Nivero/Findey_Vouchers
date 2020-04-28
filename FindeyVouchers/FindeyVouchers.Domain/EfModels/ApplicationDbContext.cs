using Microsoft.EntityFrameworkCore;

namespace FindeyVouchers.Domain.EfModels
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<MerchantVoucher> MerchantVouchers { get; set; }
    }
}