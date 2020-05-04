using System.Linq;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Website.Models
{
    public class VoucherPageResponse
    {
        public Merchant Merchant { get; set; }
        public IQueryable<MerchantVoucher>  Vouchers { get; set; }
    }

    public class Merchant
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

}