using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.WebApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        // --- ÝLÝÞKÝLER (Foreign Keys) ---

        // Randevuyu alan üye
        public int UserId { get; set; }
        public User? User { get; set; }

        // Seçilen Eðitmen
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        // Seçilen Hizmet
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        // --- BÝLGÝLER ---
        public DateTime AppointmentDate { get; set; } // Randevu Zamaný
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Ýþlem Tarihi
    }
}