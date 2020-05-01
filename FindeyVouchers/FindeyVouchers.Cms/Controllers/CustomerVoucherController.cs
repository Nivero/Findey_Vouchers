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
            var customerVouchers = _context.CustomerVouchers.Where(x => x.VoucherMerchant.Merchant == user);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                customerVouchers = customerVouchers.Where(s => s.Code.Contains(searchQuery));
            }

            var model = new CustomerVoucherViewModel
            {
                Vouchers = customerVouchers
            };

            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerVoucher = await _context.CustomerVouchers.Include(x => x.Customer)
                .Include(x => x.VoucherMerchant)
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(User);

            if (customerVoucher.VoucherMerchant.Merchant != user)
            {
                return NotFound();
            }

            return View(customerVoucher);
        }

        public async Task<IActionResult> Invalidate(Guid? id)
        {

            var customerVoucher = await _context.CustomerVouchers.FirstOrDefaultAsync(m => m.Id == id);
            if (customerVoucher != null)
            {
                try
                {
                    customerVoucher.IsUsed = true;
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Error("Error invalidating voucher with code {0}", customerVoucher.Code);
                }

            }

            return RedirectToAction("Details", "CustomerVoucher", new {id = id});

        }

        private bool CustomerVoucherExists(Guid id)
        {
            return _context.CustomerVouchers.Any(e => e.Id == id);
        }
    }
}