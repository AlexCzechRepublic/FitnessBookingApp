using FitnessBookingApp.Data;
using FitnessBookingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessBookingApp.Services
{
    public class DataService
    {
        private readonly AppDbContext _context;

        public DataService(AppDbContext context)
        {
            _context = context;
        }

        // --- USERS ---
        public List<User> GetUsers() => _context.tbUsers.ToList();

        public void AddUser(User user)
        {
            _context.tbUsers.Add(user);
            _context.SaveChanges();
        }

        public User? GetUserByName(string username)
        {
            return _context.tbUsers.FirstOrDefault(u => u.Username == username);
        }

        // --- TRAININGS ---
        public List<Training> GetTrainings()
        {
            return _context.tbTrainings
                .Include(t => t.TrainingRegistrations)
                .ThenInclude(tr => tr.User)
                .OrderBy(t => t.Date)
                .ToList();
        }

        public void AddTraining(Training training)
        {
            _context.tbTrainings.Add(training);
            _context.SaveChanges();
        }

        public void UpdateTraining(Training training)
        {
            var existing = _context.tbTrainings
                .Include(t => t.TrainingRegistrations)
                    .ThenInclude(tr => tr.User)
                .FirstOrDefault(t => t.Id == training.Id);

            if (existing != null)
            {
                existing.Date = training.Date;
                existing.DurationMinutes = training.DurationMinutes;
                existing.Capacity = training.Capacity;
                _context.SaveChanges();
            }
        }

        public void DeleteTraining(Guid id)
        {
            var training = _context.tbTrainings.FirstOrDefault(t => t.Id == id);
            if (training != null)
            {
                _context.tbTrainings.Remove(training);
                _context.SaveChanges();
            }
        }

        public void GenerateNextMonth()
        {
            var today = DateTime.Today;
            var firstDayNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
            var lastDayNextMonth = firstDayNextMonth.AddMonths(1).AddDays(-1);

            var trainings = new List<Training>();

            for (var day = firstDayNextMonth; day <= lastDayNextMonth; day = day.AddDays(1))
            {
                if (day.DayOfWeek == DayOfWeek.Monday || day.DayOfWeek == DayOfWeek.Wednesday)
                {
                    trainings.Add(new Training
                    {
                        Id = Guid.NewGuid(),
                        Date = day.AddHours(18),
                        DurationMinutes = 60,
                        Capacity = 16,
                        TrainingRegistrations = new List<TrainingRegistration>()
                    });
                }
            }

            _context.tbTrainings.AddRange(trainings);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existing = _context.tbUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                // Profilová data
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                existing.Street = user.Street;
                existing.HouseNumber = user.HouseNumber;
                existing.PostalCode = user.PostalCode;
                existing.City = user.City;

                // Kontakty (kvůli AJAX změnám e‑mailu/telefonu)
                existing.Email = user.Email;
                existing.PhoneNumber = user.PhoneNumber;

                // Username/Role/Password zde neměníme
                _context.SaveChanges();
            }
        }
    }
}