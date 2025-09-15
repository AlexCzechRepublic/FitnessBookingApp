namespace FitnessBookingApp.Models
{
    public class Database
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Training> Trainings { get; set; } = new List<Training>();
    }
}
