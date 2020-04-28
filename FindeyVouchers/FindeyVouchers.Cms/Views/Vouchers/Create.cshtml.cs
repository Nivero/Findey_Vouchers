using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindeyVouchers.Cms.Views.Vouchers
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
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
