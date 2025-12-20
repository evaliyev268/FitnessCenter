using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.WebApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Üye seçimi zorunludur")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required(ErrorMessage = "Eğitmen seçimi zorunludur")]
        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer? Trainer { get; set; }

        [Required(ErrorMessage = "Hizmet seçimi zorunludur")]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Onaylandı mı?")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Durum")]
        [StringLength(50)]
        public string Status { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Reddedildi, Tamamlandı

        [StringLength(500)]
        [Display(Name = "Notlar")]
        public string? Notes { get; set; }
    }
}