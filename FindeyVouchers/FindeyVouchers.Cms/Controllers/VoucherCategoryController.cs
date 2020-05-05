using System;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FindeyVouchers.Cms.Controllers
{
    public class VoucherCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoucherCategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VoucherCategory
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var categories = _context.VoucherCategories.Where(x => x.Merchant == user);
            return View(categories);
        }

        // GET: VoucherCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucherCategory = await _context.VoucherCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucherCategory == null)
            {
                return NotFound();
            }

            return View(voucherCategory);
        }

        // GET: VoucherCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VoucherCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] VoucherCategory voucherCategory)
        {
            if (ModelState.IsValid)
            {
                voucherCategory.Id = Guid.NewGuid();
                voucherCategory.Merchant = await _userManager.GetUserAsync(User);
                _context.Add(voucherCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voucherCategory);
        }

        // GET: VoucherCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucherCategory = await _context.VoucherCategories.FindAsync(id);
            if (voucherCategory == null)
            {
                return NotFound();
            }
            return View(voucherCategory);
        }

        // POST: VoucherCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] VoucherCategory voucherCategory)
        {
            if (id != voucherCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucherCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherCategoryExists(voucherCategory.Id))
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
            return View(voucherCategory);
        }

        private bool VoucherCategoryExists(Guid id)
        {
            return _context.VoucherCategories.Any(e => e.Id == id);
        }
    }
}
