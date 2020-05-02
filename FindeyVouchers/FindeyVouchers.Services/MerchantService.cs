using System.Linq;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;

namespace FindeyVouchers.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly ApplicationDbContext _context;

        public MerchantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetMerchantInfo(string merchantName)
        {
            var merchant = _context.Users.FirstOrDefault(x => x.CompanyName.Equals(merchantName));
            return merchant ?? null;
        }
    }
}