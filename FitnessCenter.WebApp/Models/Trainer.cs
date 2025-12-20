using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // BU SATIR ÇOK ÖNEMLİ: Eğer bu yoksa AdminController hata verir!
        public string Email { get; set; } = string.Empty;

        public string Speciality { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}