using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.WebApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Eğitmen adı zorunludur")]
        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
        [Display(Name = "Eğitmen Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur")]
        [StringLength(200)]
        [Display(Name = "Uzmanlık Alanı")]
        public string Speciality { get; set; } = string.Empty;

        [Url(ErrorMessage = "Geçerli bir URL giriniz")]
        [Display(Name = "Profil Fotoğrafı URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Biyografi")]
        public string? Bio { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;

        // YENİ ALANLAR - ÇALIŞMA SAATLERİ VE ÜCRET
        [Display(Name = "Çalışma Başlangıç Saati")]
        [DataType(DataType.Time)]
        public TimeSpan? WorkStartTime { get; set; } // Örn: 09:00

        [Display(Name = "Çalışma Bitiş Saati")]
        [DataType(DataType.Time)]
        public TimeSpan? WorkEndTime { get; set; } // Örn: 18:00

        [Display(Name = "Saatlik Ücret (TL)")]
        [Range(0, 10000, ErrorMessage = "Saatlik ücret 0-10000 TL arasında olmalıdır")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? HourlyRate { get; set; }

        [Display(Name = "Çalışma Günleri")]
        [StringLength(100)]
        public string? WorkingDays { get; set; } // Örn: "Pazartesi, Çarşamba, Cuma"

        // Trainer olarak sisteme giriş yapabilmesi için User ile ilişki
        [Display(Name = "Kullanıcı ID (Giriş için)")]
        public int? UserId { get; set; }

        [Display(Name = "E-posta (Giriş)")]
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
    }
}