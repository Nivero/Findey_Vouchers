using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Cms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Identity;

namespace FindeyVouchers.Cms.Controllers_
{
    public class CustomerVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerVoucherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string query)
        {
            var user = await _userManager.GetUserAsync(User);
            var customerVouchers = _context.CustomerVouchers.Where(x => x.VoucherMerchant.Merchant == user);

            if (!string.IsNullOrEmpty(query))
            {
                customerVouchers = customerVouchers.Where(s => s.Code.Contains(query));
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

            var customerVoucher = await _context.CustomerVouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(User);

            if (customerVoucher.VoucherMerchant.Merchant != user)
            {
                return NotFound();
            }

            return View(customerVoucher);
        }

        private bool CustomerVoucherExists(Guid id)
        {
            return _context.CustomerVouchers.Any(e => e.Id == id);
        }
    }
}