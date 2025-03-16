using ECB_ExchangeRateProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace ECB_ExchangeRateProvider.DBContexts {
    public class ExchangeRateDBContext(DbContextOptions<ExchangeRateDBContext> options) : DbContext(options) {
        public DbSet<ExchangeRateModel> ExchangeRates { get; set; }
        public DbSet<WalletModel> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeRateModel>().HasIndex(model => new { model.Currency, model.Date }).IsUnique();
        }
    }
}
