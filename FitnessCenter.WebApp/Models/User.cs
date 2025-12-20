using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.WebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100)]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz")]
        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Rol")]
        public string Role { get; set; } = "Member"; // Admin, Member
    }
}