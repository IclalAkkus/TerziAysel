using Microsoft.AspNetCore.Mvc;
using TerzimAysel.Models;
using TerzimAysel.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;

namespace TerzimAysel.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public KullaniciController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult GirisKayit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KullaniciOlustur(KullaniciViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mevcutMu = _context.Kullanicilar
                    .FirstOrDefault(k => k.KullaniciAdi == model.KullaniciAdi);

                if (mevcutMu != null)
                {
                    ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten alınmış.");
                    return View("GirisKayit", model); 
                }
                

                var kullanici = new Kullanici
                {
                    TamAd = model.TamAd,
                    Email = model.Email,
                    KullaniciAdi = model.KullaniciAdi,
                    TelefonNo = model.TelefonNo,
                    Sifre = model.Sifre 
                };

                _context.Kullanicilar.Add(kullanici);
                _context.SaveChanges();

                TempData["UserName"] = kullanici.KullaniciAdi;

                TempData["SuccessMessage"] = "Kullanıcı başarıyla kaydedildi!";
                
                return RedirectToAction("Index", "Home");
            }
            return View("GirisKayit", model);
        }

        [HttpPost]
        public IActionResult Giris(KullaniciViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("GirisKayit");
            }

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => 
                k.Email == model.EmailOrKullaniciAdi || 
                k.KullaniciAdi == model.EmailOrKullaniciAdi);

            if (kullanici == null || kullanici.Sifre != model.Sifre)
            {
                TempData["ErrorMessage"] = "Geçersiz e-posta, kullanıcı adı veya şifre.";
                return View("GirisKayit");
            }

            // Oturum bilgilerini ayarla
            HttpContext.Session.SetString("UserId", kullanici.KullaniciId.ToString());
            HttpContext.Session.SetString("UserName", kullanici.KullaniciAdi);
            TempData["UserName"] = kullanici.KullaniciAdi;

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult YorumEkle(string yorumMetni, string kategori)
        {
            try
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Json(new { success = false, message = "Yorum yapmak için giriş yapmalısınız." });
                }

                var userName = HttpContext.Session.GetString("UserName");
                var yorum = new Yorum
                {
                    KullaniciId = userId,
                    YorumMetni = yorumMetni,
                    kategoriler = kategori,
                    Tarih = DateTime.Now,
                };

                _context.Yorumlar.Add(yorum);
                _context.SaveChanges();

                return Json(new { success = true, yorum = new { kullaniciAdi = userName, tarih = yorum.Tarih.ToString("dd.MM.yyyy HH:mm"), yorumMetni = yorumMetni } });
            }
            catch
            {
                return Json(new { success = false, message = "Yorum eklenirken bir hata oluştu." });
            }
        }
    


       


        [HttpPost]
        public IActionResult CreateIletisim(IletisimViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var iletisim = new Iletisim
                    {
                        TamAd = model.TamAd,
                        Email = model.Email,
                        TelefonNo = model.TelefonNo,
                        Mesaj = model.Mesaj
                    };

                    _context.Iletisimler.Add(iletisim);
                    _context.SaveChanges();

                    using (var smtp = new SmtpClient())
                    {
                        var mailMessage = new MailMessage();
                        mailMessage.From = new MailAddress("terzimayseloffical@gmail.com", "Aysel AKKUŞ");
                        mailMessage.To.Add("terzimayseloffical@gmail.com");
                        mailMessage.Subject = "Yeni İletişim Formu Mesajı";
                        mailMessage.Body = $@"
                            Gönderen Bilgileri:
                            Ad Soyad: {model.TamAd}
                            E-posta: {model.Email}
                            Telefon: {model.TelefonNo}

                            Mesaj:
                            {model.Mesaj}
                        ";
                        mailMessage.IsBodyHtml = true;

                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(
                            "terzimayseloffical@gmail.com",
                            "xchv hsns cfop sbna"
                        );

                        smtp.Send(mailMessage);
                    }

                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Hata detayını loglayalım
                Console.WriteLine($"Mail gönderme hatası: {ex.Message}");
                return Json(new { success = false });
            }
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}