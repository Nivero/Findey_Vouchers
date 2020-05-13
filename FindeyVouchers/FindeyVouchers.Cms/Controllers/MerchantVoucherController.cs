using System;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Cms.Models;
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
                return RedirectToAction("Index", "Home");


            var vouchers = await _context.MerchantVouchers.Include(x => x.Category).Where(x => x.Merchant == user)
                .ToListAsync();
            foreach (var item in vouchers)
                item.AmountSold = _context.CustomerVouchers.Count(x => x.MerchantVoucher == item);

            return View(vouchers);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var merchantVoucher = await _context.MerchantVouchers.Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (merchantVoucher == null) return NotFound();

            return View(merchantVoucher);
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new MerchantVoucherViewModel
            {
                Voucher = new MerchantVoucher(),
                Categories = _voucherService.GetCategories(user)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MerchantVoucherViewModel merchantVoucher,
            [FromForm(Name = "ImageFile")] IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            // TODO: clean this mess up. I couldnt get the category to bind to the voucher model.
            // So i added the id to the viewmodel and added it by hand here.
            merchantVoucher.Voucher.Category =
                _context.VoucherCategories.FirstOrDefault(x => x.Id == merchantVoucher.CategoryId);
            if (ModelState.IsValid)
            {
                if (merchantVoucher.Voucher.DefaultImages == DefaultImages.Default)
                    await _voucherService.CreateMerchantVoucher(merchantVoucher.Voucher, file, user);
                else
                    await _voucherService.CreateMerchantVoucher(merchantVoucher.Voucher,
                        merchantVoucher.Voucher.DefaultImages, user);

                return RedirectToAction(nameof(Index));
            }

            merchantVoucher.Categories = _voucherService.GetCategories(user);
            return View(merchantVoucher);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var merchantVoucher =
                _context.MerchantVouchers.Include(x => x.Category)
                    .FirstOrDefault(x => x.Id == id);
            if (merchantVoucher == null) return NotFound();

            var model = new MerchantVoucherViewModel
            {
                Voucher = merchantVoucher,
                Categories = _voucherService.GetCategories(user),
                CategoryId = merchantVoucher.Category.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MerchantVoucherViewModel merchantVoucher,
            [FromForm(Name = "ImageFile")] IFormFile file)
        {
            if (id != merchantVoucher.Voucher.Id) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            // TODO: clean this mess up. I couldnt get the category to bind to the voucher model.
            // So i added the id to the viewmodel and added it by hand here.
            merchantVoucher.Voucher.Category =
                _context.VoucherCategories.FirstOrDefault(x => x.Id == merchantVoucher.CategoryId);
            if (ModelState.IsValid)
            {
                if (merchantVoucher.Voucher.DefaultImages == DefaultImages.Default)
                    await _voucherService.UpdateMerchantVoucher(merchantVoucher.Voucher, file);
                else
                    await _voucherService.UpdateMerchantVoucher(merchantVoucher.Voucher,
                        merchantVoucher.Voucher.DefaultImages);


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