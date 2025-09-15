using System.ComponentModel.DataAnnotations;

namespace FitnessBookingApp.Models
{
    public class Training
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public int Capacity { get; set; }

        public List<TrainingRegistration> TrainingRegistrations { get; set; } = new();
    }
}
