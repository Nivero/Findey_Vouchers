using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Views.Vouchers
{
    public class EditModel : PageModel
    {
        private readonly FindeyVouchers.Domain.EfModels.ApplicationDbContext _context;

        public EditModel(FindeyVouchers.Domain.EfModels.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MerchantVoucher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MerchantVoucherExists(MerchantVoucher.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MerchantVoucherExists(Guid id)
        {
            return _context.MerchantVouchers.Any(e => e.Id == id);
        }
    }
}
