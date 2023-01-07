using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Migrations;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Data
{
    public class YgoContext : DbContext
    {
        public YgoContext(DbContextOptions options) : base(options)
        {

        }
        public YgoContext()
        {

        }
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
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;");
        }

    }
}
