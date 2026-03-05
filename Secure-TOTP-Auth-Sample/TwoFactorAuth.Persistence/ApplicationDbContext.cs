using Secure_TOTP_Auth_Sample.TwoFactorAuth.Domain.Entities;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RecoveryCode> RecoveryCodes => Set<RecoveryCode>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Yapılandırması
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Email).IsRequired().HasMaxLength(256);

                // 2FA Secret'ları için uzunluk kısıtlamaları
                builder.Property(u => u.TwoFactorSecret).HasMaxLength(512);
                builder.Property(u => u.PendingTwoFactorSecret).HasMaxLength(512);

                // İlişki: Bir kullanıcının birden fazla kurtarma kodu olabilir
                builder.HasMany(u => u.RecoveryCodes)
                       .WithOne()
                       .HasForeignKey(rc => rc.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            // RecoveryCode Yapılandırması
            modelBuilder.Entity<RecoveryCode>(builder =>
            {
                builder.HasKey(rc => rc.Id);
                builder.Property(rc => rc.CodeHash).IsRequired();
            });
        }
    }
}
