using Microsoft.EntityFrameworkCore;
using QuickPay.Models.Domain;
using System.Transactions;

namespace QuickPay.Data
{
    public class QuickPayDbContext : DbContext
    {
        public QuickPayDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Transactions> Transactions { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.SenderWallet)
                .WithMany()
                .HasForeignKey(t => t.SenderWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.ReceiverWallet)
                .WithMany()
                .HasForeignKey(t => t.ReceiverWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }


    }
}
