namespace TerzimAysel.Models
{
    public class Yorum
    {
        public int YorumId { get; set; }              // Yorumun benzersiz kimliği
        public int KullaniciId { get; set; }           // Yorum yapan kullanıcının kimliği
        public Kullanici? Kullanici { get; set; }      // Kullanıcı bilgisi (User ile ilişki)
        public string YorumMetni { get; set; } = string.Empty; // Yorumun metni
        public DateTime Tarih { get; set; } = DateTime.Now;   // Yorumun yapıldığı tarih
        public string kategoriler { get; set; } = string.Empty;
    }
}
