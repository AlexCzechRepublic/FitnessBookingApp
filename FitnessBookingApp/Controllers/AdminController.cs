using Microsoft.AspNetCore.Mvc;
using FitnessBookingApp.Models;
using FitnessBookingApp.Services;

namespace FitnessBookingApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly DataService _dataService;
        public AdminController(DataService dataService) : base(dataService)
        {
            _dataService = dataService;
        }       


        // Přehled tréninků
        public IActionResult Trainings()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            var trainings = _dataService.GetTrainings();
            ViewBag.Role = "Admin";
            return View(trainings);
        }

        // Přehled uživatelů
        public IActionResult Users()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            var users = _dataService.GetUsers();
            return View(users);
        }

        // Odhlášení uživatele z tréninku (admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnregisterUser(int userId, Guid trainingId)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();

            var training = _dataService.GetTrainings().FirstOrDefault(t => t.Id == trainingId);
            if (training == null) return NotFound();
            var registration = training.TrainingRegistrations.FirstOrDefault(tr => tr.UserId == userId);
            if (registration != null)
            {
                training.TrainingRegistrations.Remove(registration);
                _dataService.UpdateTraining(training);
            }
            return RedirectToAction("Trainings");
        }

        [HttpGet]
        public IActionResult UserDetail(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            var user = _dataService.GetUsers().FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            var entries = _dataService.GetEntriesForUser(id);
            var balance = _dataService.GetBalance(id);
            ViewBag.Balance = balance;
            ViewBag.User = user;
            return View("~/Views/Admin/UserDetail.cshtml", entries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEntry(int userId, decimal amount, string note)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            _dataService.AddEntry(userId, amount, note, "manual");
            return RedirectToAction("UserDetail", new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveEntry(int entryId, int userId)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            _dataService.RemoveEntry(entryId);
            return RedirectToAction("UserDetail", new { id = userId });
        }
    }
}