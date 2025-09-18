using Microsoft.AspNetCore.Mvc;
using FitnessBookingApp.Models;
using FitnessBookingApp.Services;

namespace FitnessBookingApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly DataService _dataService;

        public HomeController(DataService dataService) : base(dataService)     
        {
            _dataService = dataService;
            if (!_dataService.GetTrainings().Any())
            {
                GenerateDefaultTrainings();
            }
        }
        private void GenerateDefaultTrainings()
        {
            var startDate = DateTime.Today;
            for (int i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i);
                if (date.DayOfWeek == DayOfWeek.Monday || date.DayOfWeek == DayOfWeek.Wednesday)
                {
                    var training = new Training
                    {
                        Id = Guid.NewGuid(),
                        Date = date.AddHours(18),
                        DurationMinutes = 60,
                        Capacity = 16,
                        TrainingRegistrations = new List<TrainingRegistration>() // DB-friendly
                    };
                    _dataService.AddTraining(training);
                }
            }
        }



        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View(_dataService.GetTrainings());
        }

        [HttpPost]
        public IActionResult Register(Guid id)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var training = _dataService.GetTrainings().FirstOrDefault(t => t.Id == id);
            var user = _dataService.GetUserByName(username);

            if (training != null && user != null &&
                !training.TrainingRegistrations.Any(tr => tr.UserId == user.Id) &&
                training.TrainingRegistrations.Count < training.Capacity)
            {
                var registration = new TrainingRegistration
                {
                    UserId = user.Id,
                    User = user,
                    TrainingId = training.Id,
                    Training = training
                };

                training.TrainingRegistrations.Add(registration);
                _dataService.UpdateTraining(training);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Unregister(Guid id)
        {
            var username = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var training = _dataService.GetTrainings().FirstOrDefault(t => t.Id == id);
            var user = _dataService.GetUserByName(username);

            if (training != null && user != null)
            {
                var registration = training.TrainingRegistrations.FirstOrDefault(tr => tr.UserId == user.Id);
                if (registration != null)
                {
                    training.TrainingRegistrations.Remove(registration);
                    _dataService.UpdateTraining(training);
                }
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Training model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();

            var newTraining = new Training
            {
                Id = Guid.NewGuid(),
                Date = model.Date,
                DurationMinutes = 60,
                Capacity = model.Capacity,
                TrainingRegistrations = new List<TrainingRegistration>() // DB-friendly
            };

            _dataService.AddTraining(newTraining);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();

            var training = _dataService.GetTrainings().FirstOrDefault(t => t.Id == id);
            if (training == null) return NotFound();

            return View(training);
        }

        [HttpPost]
        public IActionResult Edit(Training model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();

            var training = _dataService.GetTrainings().FirstOrDefault(t => t.Id == model.Id);
            if (training != null)
            {
                training.Date = model.Date;
                training.DurationMinutes = model.DurationMinutes;
                training.Capacity = model.Capacity;
                _dataService.UpdateTraining(training);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Unauthorized();
            _dataService.DeleteTraining(id);
            return RedirectToAction("Index");
        }
    }
}
