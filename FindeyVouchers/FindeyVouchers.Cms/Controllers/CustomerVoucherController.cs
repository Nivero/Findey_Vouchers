using System;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Cms.Models;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Serilog;

namespace FindeyVouchers.Cms.Controllers
{
    [Authorize]
    public class CustomerVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVoucherService _voucherService;

        public CustomerVoucherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IVoucherService voucherService)
        {
            _userManager = userManager;
            _voucherService = voucherService;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrWhiteSpace(user.CompanyName) || string.IsNullOrWhiteSpace(user.StripeAccountId))
                return RedirectToAction("Index", "Home");

            var customerVouchers = _context.CustomerVouchers.Include(x => x.Payment)
                .Where(x => x.MerchantVoucher.Merchant == user);

            if (!string.IsNullOrEmpty(searchQuery))
                customerVouchers = customerVouchers.Include(x => x.Payment).Where(s => s.Code.Contains(searchQuery));

            var model = new CustomerVoucherViewModel
            {
                Vouchers = customerVouchers
            };

            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var customerVoucher = await _context.CustomerVouchers.Include(x => x.Customer)
                .Include(x => x.MerchantVoucher)
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(User);

            if (customerVoucher.MerchantVoucher.Merchant != user) return NotFound();

            return View(customerVoucher);
        }

        public RedirectToActionResult Invalidate(Guid? id)
        {
            if (id != null)
                try
                {
                    _voucherService.InvalidateCustomerVoucher(id.Value);
                }
                catch (Exception e)
                {
                    Log.Error("Error invalidating voucher with id {0} error: {1}", id, e);
                }

            return RedirectToAction("Details", "CustomerVoucher", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetractAmount(CustomerVoucher customerVoucher)
        {            var model = await _context.CustomerVouchers.Include(x => x.Customer)
                .Include(x => x.MerchantVoucher)
                .FirstOrDefaultAsync(m => m.Id == customerVoucher.Id);
            if (ModelState.IsValid)
            {
                var currentVoucher = _context.CustomerVouchers.FirstOrDefault(x => customerVoucher.Id == x.Id);
                if (customerVoucher != null && currentVoucher != null)
                {
                    if (currentVoucher.Price > customerVoucher.Price)
                    {
                        ModelState.AddModelError(String.Empty, "Bedrag kan niet hoger zijn dan huidige waarde.");
                        return View("Details", model);
                    }

                    var newPrice = currentVoucher.Price - customerVoucher.Price;
                    _voucherService.UpdatePrice(customerVoucher.Id, newPrice);
                    return RedirectToAction("Details", "CustomerVoucher", new {id = customerVoucher.Id});
                }
            }
            
            return View("Details", model);
        }

        private bool CustomerVoucherExists(Guid id)
        {
            return _context.CustomerVouchers.Any(e => e.Id == id);
        }
    }
}