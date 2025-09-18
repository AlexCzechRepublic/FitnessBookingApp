using Microsoft.AspNetCore.Mvc;
using FitnessBookingApp.Models;
using FitnessBookingApp.Services;
using Microsoft.AspNetCore.Identity;
using FitnessBookingApp.Utils;

namespace FitnessBookingApp.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly DataService _dataService;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public ProfileController(DataService dataService) : base(dataService)
        {
            _dataService = dataService;
        }       


        [HttpGet]
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            ViewBag.Success = TempData["Success"];
            return View("~/Views/Account/Profile.cshtml", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(ProfileUpdateViewModel model)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Street = model.Street;
                user.HouseNumber = model.HouseNumber;
                user.PostalCode = model.PostalCode;
                user.City = model.City;

                return View("~/Views/Account/Profile.cshtml", user);
            }

            // Přepiš profilová pole a ulož
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Street = model.Street;
            user.HouseNumber = model.HouseNumber;
            user.PostalCode = model.PostalCode;
            user.City = model.City;

            _dataService.UpdateUser(user);

            TempData["Success"] = "Profil byl úspěšně aktualizován!";
            return RedirectToAction(nameof(Profile));
        }

        // AJAX: telefon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePhone(string PhoneNumber)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return BadRequest("Není přihlášen uživatel.");

            if (!PhoneUtils.TryNormalizeFull(PhoneNumber, out var normalized, out var error))
                return BadRequest(error);

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            user.PhoneNumber = normalized;
            _dataService.UpdateUser(user);

            return Ok(PhoneUtils.FormatDisplay(normalized));
        }

        // AJAX: e-mail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateEmail(string Email)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return BadRequest("Není přihlášen uživatel.");

            if (string.IsNullOrWhiteSpace(Email) || !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email))
                return BadRequest("Neplatný e‑mail.");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            user.Email = Email.Trim();
            _dataService.UpdateUser(user);
            return Ok();
        }

        [HttpGet]
        public IActionResult Balance()
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var user = _dataService.GetUserByName(username);
            if (user == null) return NotFound();

            var entries = _dataService.GetEntriesForUser(user.Id);
            var balance = _dataService.GetBalance(user.Id);

            ViewBag.Balance = balance;
            return View("~/Views/Account/Balance.cshtml", entries);
        }
    }
}