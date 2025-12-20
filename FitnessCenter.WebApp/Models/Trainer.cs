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
    }
}