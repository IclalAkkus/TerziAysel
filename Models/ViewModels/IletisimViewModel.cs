using System.ComponentModel.DataAnnotations;

namespace TerzimAysel.Models
{
    public class IletisimViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [Display(Name = "Ad Soyad")]
        public string TamAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon Numarası")]
        public string TelefonNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj zorunludur")]
        [MinLength(10, ErrorMessage = "Mesajınız en az 10 karakter olmalıdır")]
        public string Mesaj { get; set; } = string.Empty;
    }
}