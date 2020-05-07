using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace FindeyVouchers.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public MerchantService(ApplicationDbContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public ApplicationUser GetMerchantInfo(string merchantName)
        {
            if (merchantName != null)
            {
                var merchant =
                    _context.Users.FirstOrDefault(x => x.NormalizedCompanyName.Equals(merchantName.ToLower()));
                return merchant ?? null;
            }

            return null;
        }
    }
}