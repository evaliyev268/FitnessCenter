namespace FitnessCenter.WebApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        // Hizmet Adý (Örn: Pilates, Birebir Fitness)
        public string Name { get; set; } = string.Empty;

        // Açýklama (Örn: Esneklik ve denge için birebir ders)
        public string Description { get; set; } = string.Empty;

        // Süre (Dakika cinsinden, Örn: 60)
        public int Duration { get; set; }

        // Ücret (Örn: 500 TL)
        public decimal Price { get; set; }

        // Hizmetin Resmi
        public string ImageUrl { get; set; } = string.Empty;
    }
}