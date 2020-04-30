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
    public class VoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAzureStorageService _azureStorageService;

        public VoucherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IAzureStorageService azureStorageService)
        {
            _context = context;
            _userManager = userManager;
            _azureStorageService = azureStorageService;
        }

        // GET: Voucher

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrWhiteSpace(user.CompanyName) || string.IsNullOrWhiteSpace(user.StripeAccountId))
            {
                return RedirectToAction("Index", "Home");
            }

            var vouchers = await _context.MerchantVouchers.Where(x => x.Merchant == user).ToListAsync();
            return View(vouchers);
        }

        // GET: Voucher/Details/5

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

        public IActionResult Create()
        {
            return View();
        }

        // POST: Voucher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MerchantVoucher merchantVoucher,
            [FromForm(Name = "ImageFile")] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                merchantVoucher.Id = Guid.NewGuid();
                merchantVoucher.Merchant = await _userManager.GetUserAsync(User);

                // Upload file
                merchantVoucher.Image = await UploadFile(file);
                _context.Add(merchantVoucher);

                await _context.SaveChangesAsync();
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

        // POST: Voucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

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
                try
                {
                    if (file != null)
                    {
                        _azureStorageService.DeleteBlobData(merchantVoucher.Image);
                        merchantVoucher.Image = await UploadFile(file);
                    }

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


        public async Task<IActionResult> ChangeActive(Guid? id)
        {
            var merchantVoucher = await _context.MerchantVouchers.FindAsync(id);
            merchantVoucher.IsActive = !merchantVoucher.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool MerchantVoucherExists(Guid id)
        {
            return _context.MerchantVouchers.Any(e => e.Id == id);
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] bytes = memoryStream.ToArray();

                var result =
                    await _azureStorageService.UploadFileToBlobAsync(file.FileName, bytes, file.ContentType);
                return result;
            }
        }
    }
}