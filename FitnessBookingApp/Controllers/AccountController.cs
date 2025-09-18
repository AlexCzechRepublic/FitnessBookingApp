using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using FitnessBookingApp.Models;
using FitnessBookingApp.Services;

namespace FitnessBookingApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataService _dataService;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AccountController(DataService dataService) :base(dataService)
        {
            _dataService = dataService;
        }       

        // --- LOGIN GET ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // --- LOGIN POST ---
        [HttpPost]
        public IActionResult Login(User user)
        {
            var foundUser = _dataService.GetUsers().FirstOrDefault(u => u.Username == user.Username);

            if (foundUser != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(foundUser, foundUser.Password, user.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("User", foundUser.Username);
                    HttpContext.Session.SetString("Role", foundUser.Role);
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Neplatné přihlašovací údaje!";
            return View();
        }

        // --- LOGOUT ---
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // --- REGISTER GET ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_dataService.GetUsers().Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "Uživatel s tímto jménem už existuje!");
                return View(model);
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = _passwordHasher.HashPassword(null, model.Password),
                Role = "User"
            };

            _dataService.AddUser(newUser);

            HttpContext.Session.SetString("User", newUser.Username);
            HttpContext.Session.SetString("Role", newUser.Role);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _dataService.GetUsers().FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound();

            return View(user);
        }
    }
}
