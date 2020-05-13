using System.Linq;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Models
{
    public class CustomerVoucherViewModel
    {
        public IQueryable<CustomerVoucher> Vouchers { get; set; }
        public string SearchQuery { get; set; }
    }
}