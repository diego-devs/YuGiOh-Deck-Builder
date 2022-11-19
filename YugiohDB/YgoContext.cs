using Microsoft.EntityFrameworkCore;
using YugiohDB.Models;

namespace YugiohDB
{
    public class YgoContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<CardSet> CardSets { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<SetInfo> SetsInfo { get; set; }
        public DbSet<BanlistInfo> CardsBanlist { get; set; }
        public DbSet<MiscInfo> MiscInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }



    }
}