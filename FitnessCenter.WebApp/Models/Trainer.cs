namespace FitnessCenter.WebApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty; // Uzmanlık alanı
        public string ImageUrl { get; set; } = string.Empty;
    }
}