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
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(user.CompanyName) || string.IsNullOrWhiteSpace(user.StripeAccountId))
            {
                return RedirectToAction("Index", "Home");
            }

            var applicationUser = await _context.Users.FindAsync(user.Id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string id,
            ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    user.FirstName = applicationUser.FirstName;
                    user.LastName = applicationUser.LastName;
                    user.DateOfBirth = applicationUser.DateOfBirth;
                    user.Address = applicationUser.Address;
                    user.ZipCode = applicationUser.ZipCode;
                    user.City = applicationUser.City;
                    user.BusinessType = applicationUser.BusinessType;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.Email = applicationUser.Email;
                    user.NormalizedEmail = applicationUser.Email.ToUpper();
                    user.CompanyName = applicationUser.CompanyName;

                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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

            return View(applicationUser);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}