using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.WebApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur")]
        [StringLength(100)]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Süre zorunludur")]
        [Range(15, 300, ErrorMessage = "Süre 15-300 dakika arasında olmalıdır")]
        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Ücret zorunludur")]
        [Range(0, 100000, ErrorMessage = "Ücret 0-100000 TL arasında olmalıdır")]
        [Display(Name = "Ücret (TL)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Geçerli bir resim URL'si giriniz")]
        [Display(Name = "Görsel URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;
    }
}