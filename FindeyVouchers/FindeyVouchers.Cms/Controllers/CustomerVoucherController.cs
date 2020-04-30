using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Cms.Controllers_
{
    public class CustomerVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerVoucherController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerVouchers.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerVoucher = await _context.CustomerVouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerVoucher == null)
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
