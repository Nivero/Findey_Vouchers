using System;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FindeyVouchers.Cms.Controllers
{
    public class VoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoucherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Voucher
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var vouchers = await _context.MerchantVouchers.Where(x => x.Merchant == user).ToListAsync();
            return View(vouchers);
        }

        // GET: Voucher/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchantVoucher = await _context.MerchantVouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (merchantVoucher == null)
            {
                return NotFound();
            }

            return View(merchantVoucher);
        }

        // GET: Voucher/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voucher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Image,IsActive,CreatedOn,ValidUntil,Price")]
            MerchantVoucher merchantVoucher)
        {
            if (ModelState.IsValid)
            {
                merchantVoucher.Id = Guid.NewGuid();
                merchantVoucher.Merchant = await _userManager.GetUserAsync(User);
                _context.Add(merchantVoucher);
                ;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(merchantVoucher);
        }

        // GET: Voucher/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchantVoucher = await _context.MerchantVouchers.FindAsync(id);
            if (merchantVoucher == null)
            {
                return NotFound();
            }

            return View(merchantVoucher);
        }

        // POST: Voucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Id,Name,Description,Image,IsActive,CreatedOn,ValidUntil,Price")]
            MerchantVoucher merchantVoucher)
        {
            if (id != merchantVoucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchantVoucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchantVoucherExists(merchantVoucher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(merchantVoucher);
        }

        [Authorize]
        public async Task<IActionResult> ChangeActive(Guid? id)
        {
            var merchantVoucher = await _context.MerchantVouchers.FindAsync(id);
            merchantVoucher.IsActive = !merchantVoucher.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool MerchantVoucherExists(Guid id)
        {
            return _context.MerchantVouchers.Any(e => e.Id == id);
        }
    }
}