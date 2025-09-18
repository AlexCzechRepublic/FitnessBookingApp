using System;

namespace FitnessBookingApp.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }   // poznámka/admin poznámka
        public string Type { get; set; }   // "manual", "stripe", ...
    }
}