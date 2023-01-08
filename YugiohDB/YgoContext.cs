using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using YugiohDB.Models;

namespace YugiohDB
{
    public class YgoContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardPrices> Prices { get; set; }
        public DbSet<CardSet> CardSets { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<CardImages> Images { get; set; }
        public DbSet<SetInfo> SetsInfo { get; set; }
        public DbSet<BanlistInfo> CardsBanlist { get; set; }
        public DbSet<MiscInfo> MiscInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }



    }
}