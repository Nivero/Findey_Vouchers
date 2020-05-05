using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FindeyVouchers.Cms.Controllers
{
    [Authorize]
    public class MerchantVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVoucherService _voucherService;

        public MerchantVoucherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IVoucherService voucherService)
        {
            _context = context;
            _userManager = userManager;
            _voucherService = voucherService;
        }

        public async Task<IActionResult> Index(string query)
        {
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrWhiteSpace(user.CompanyName) || string.IsNullOrWhiteSpace(user.StripeAccountId))
            {
                return RedirectToAction("Index", "Home");
            }


            var vouchers = await _context.MerchantVouchers.Where(x => x.Merchant == user).ToListAsync();
            foreach (var item in vouchers)
            {
                item.AmountSold = _context.CustomerVouchers.Count(x => x.VoucherMerchant == item);
            }

            return View(vouchers);
        }

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MerchantVoucher merchantVoucher,
            [FromForm(Name = "ImageFile")] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (merchantVoucher.DefaultImages == DefaultImages.Default)
                {
                    await _voucherService.CreateMerchantVoucher(merchantVoucher, file, user);
                }
                else
                {
                    await _voucherService.CreateMerchantVoucher(merchantVoucher, merchantVoucher.DefaultImages, user);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(merchantVoucher);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MerchantVoucher merchantVoucher,
            [FromForm(Name = "ImageFile")] IFormFile file)
        {
            if (id != merchantVoucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (merchantVoucher.DefaultImages == DefaultImages.Default)
                {
                    await _voucherService.UpdateMerchantVoucher(merchantVoucher, file);
                }
                else
                {
                    await _voucherService.UpdateMerchantVoucher(merchantVoucher, merchantVoucher.DefaultImages);
                }
                

                return RedirectToAction(nameof(Index));
            }

            return View(merchantVoucher);
        }


        public async Task<IActionResult> ChangeActive(Guid? id)
        {
            if (id != null) await _voucherService.DeactivateMerchantVoucher(id.Value);

            return RedirectToAction(nameof(Index));
        }


        private bool MerchantVoucherExists(Guid id)
        {
            return _context.MerchantVouchers.Any(e => e.Id == id);
        }
    }
}