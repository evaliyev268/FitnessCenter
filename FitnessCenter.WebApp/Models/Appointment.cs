using System;
using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // YENÝ: Randevu Durumu (Bekliyor, Onaylandý, Reddedildi)
        public string Status { get; set; } = "Bekliyor";
    }
}