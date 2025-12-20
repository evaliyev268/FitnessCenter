namespace FitnessCenter.WebApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        // Hizmet Adý
        public string Name { get; set; } = string.Empty;

        // Açýklama
        public string Description { get; set; } = string.Empty;

        // Süre (Dakika)
        public int Duration { get; set; }

        // Ücret
        public decimal Price { get; set; }

        // Hizmetin Resmi
        public string ImageUrl { get; set; } = string.Empty;

        // YENÝ: Çalýþma Saatleri (Örn: 09:00 - 21:00)
        public string WorkingHours { get; set; } = "09:00 - 22:00";
    }
}