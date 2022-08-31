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
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<PriceModel> CardPrices { get; set; }
        public DbSet<SetModel> CardSets { get; set; }
        public DbSet<CardImage> CardImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)     
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Trusted_Connection=True;");
        }

    }
}
