using Microsoft.EntityFrameworkCore;
using YugiohDB.Models;

namespace YugiohDB 
{
    public class YugiohContext : DbContext 
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardPrice> CardPrices { get; set; }
        public DbSet<CardSet> CardSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YugiohDatabase;Trusted_Connection=True;");
        }

    }
}