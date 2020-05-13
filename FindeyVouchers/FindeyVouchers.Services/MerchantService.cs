using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Serilog;

namespace FindeyVouchers.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public MerchantService(ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public ApplicationUser GetMerchantInfo(string merchantName)
        {
            if (merchantName != null)
            {
                var merchant =
                    _context.Users.FirstOrDefault(x => x.NormalizedCompanyName.Equals(merchantName.ToLower()));
                return merchant;
            }

            return null;
        }

        public async Task CreateAndSendMerchantNotification(List<CustomerVoucher> vouchers)
        {
            try
            {
                var merchantVouchers = vouchers.Select(x => x.MerchantVoucher).Distinct();
                var sb = new StringBuilder();
                foreach (var merchantVoucher in merchantVouchers)
                {
                    var count = vouchers.Count(x => x.MerchantVoucher == merchantVoucher);
                    var emailVoucher = _mailService.GetVoucherNoticiationHtml(merchantVoucher, count);
                    sb.Append(emailVoucher);
                }

                var totalAmount = vouchers.Sum(x => x.Price);
                var subject = "Nieuwe bestelling via Findey Vouchers";
                var body = _mailService.GetVoucherNotificationHtmlBody(
                    vouchers.First().MerchantVoucher.Merchant.CompanyName, sb.ToString(), totalAmount,
                    vouchers.Count());
                var response =
                    await _mailService.SendMail(vouchers.First().MerchantVoucher.Merchant.Email, subject, body);
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }
        }
    }
}