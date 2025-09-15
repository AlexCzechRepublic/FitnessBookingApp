using Microsoft.AspNetCore.Mvc;
using FitnessBookingApp.Models;
using FitnessBookingApp.Services;
using Microsoft.AspNetCore.Identity;

namespace FitnessBookingApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataService _dataService;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public ProfileController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            // Explicitně odkaz na view v Account
            return View("~/Views/Account/Profile.cshtml", user);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            return View(user); // Views/Account/Profile.cshtml
        }

        [HttpPost]
        public IActionResult Profile(User model)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            // Aktualizujeme jen klasická pole
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Street = model.Street;
            user.HouseNumber = model.HouseNumber;
            user.PostalCode = model.PostalCode;
            user.City = model.City;

            _dataService.UpdateUser(user);
            ViewBag.Success = "Profil byl úspěšně aktualizován!";
            return View("~/Views/Account/Profile.cshtml", user);
        }

        // AJAX endpoints pro email a telefon
        [HttpPost]
        public IActionResult UpdateEmail(string Email)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return BadRequest();

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            user.Email = Email;
            _dataService.UpdateUser(user);
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdatePhone(string PhoneNumber)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return BadRequest();

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            user.PhoneNumber = PhoneNumber;
            _dataService.UpdateUser(user);
            return Ok();
        }

    }
}
