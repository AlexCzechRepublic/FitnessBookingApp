namespace FitnessBookingApp.Models
{
    public class TrainingRegistration
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid TrainingId { get; set; }
        public Training Training { get; set; } = null!;
    }
}
