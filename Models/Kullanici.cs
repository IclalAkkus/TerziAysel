using System.ComponentModel.DataAnnotations;

namespace TerzimAysel.Models
{
    public class Kullanici
    {
    public int KullaniciId { get; set; }

    [Required(ErrorMessage = "Ad Soyad zorunludur")]
    [Display(Name = "Ad Soyad")]
    public string TamAd { get; set; } =string.Empty;

    [Required(ErrorMessage = "E-posta adresi zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    public string Email { get; set; }=string.Empty;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
    [Display(Name = "Kullanıcı Adı")]
    public string KullaniciAdi { get; set; }=string.Empty;
    public string TelefonNo { get; set; }=string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur")]
    [DataType(DataType.Password)]
    public string Sifre { get; set; }=string.Empty;
    public ICollection<Yorum>? Yorumlar { get; set; }
    }
}
