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
    public class DeleteModel : PageModel
    {
        private readonly FindeyVouchers.Domain.EfModels.ApplicationDbContext _context;

        public DeleteModel(FindeyVouchers.Domain.EfModels.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MerchantVoucher MerchantVoucher { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MerchantVoucher = await _context.MerchantVouchers.FirstOrDefaultAsync(m => m.Id == id);

            if (MerchantVoucher == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MerchantVoucher = await _context.MerchantVouchers.FindAsync(id);

            if (MerchantVoucher != null)
            {
                _context.MerchantVouchers.Remove(MerchantVoucher);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
