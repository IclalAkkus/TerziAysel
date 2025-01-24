using Microsoft.EntityFrameworkCore;
using TerzimAysel.Models;

namespace TerzimAysel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public required DbSet<Kullanici> Kullanicilar { get; set; }
        public required DbSet<Yorum> Yorumlar { get; set; }
        public required DbSet<Iletisim> Iletisimler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.HasKey(e => e.KullaniciId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Sifre).IsRequired();
                entity.Property(e => e.KullaniciAdi).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.KullaniciAdi).IsUnique();
            });

            modelBuilder.Entity<Yorum>(entity =>
            {
                entity.HasKey(e => e.YorumId);
                entity.Property(e => e.YorumMetni).IsRequired();
                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.Yorumlar)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.kategoriler).HasMaxLength(50).IsRequired(false); // 50 karakter sınırı
            });

            modelBuilder.Entity<Iletisim>(entity =>
            {
                entity.HasKey(e => e.IletisimId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Mesaj).IsRequired();
            });
        }
    }
}