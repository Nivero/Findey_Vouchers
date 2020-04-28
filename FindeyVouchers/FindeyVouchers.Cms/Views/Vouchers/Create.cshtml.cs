using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Views.Vouchers
{
    public class CreateModel : PageModel
    {
        private readonly FindeyVouchers.Domain.EfModels.ApplicationDbContext _context;

        public CreateModel(FindeyVouchers.Domain.EfModels.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MerchantVoucher MerchantVoucher { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MerchantVouchers.Add(MerchantVoucher);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
