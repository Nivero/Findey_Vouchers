using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Views.Vouchers
{
    public class IndexModel : PageModel
    {
        private readonly FindeyVouchers.Domain.EfModels.ApplicationDbContext _context;

        public IndexModel(FindeyVouchers.Domain.EfModels.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MerchantVoucher> MerchantVoucher { get;set; }

        public async Task OnGetAsync()
        {
            MerchantVoucher = await _context.MerchantVouchers.ToListAsync();
        }
    }
}
