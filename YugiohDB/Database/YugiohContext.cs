using Microsoft.EntityFrameworkCore;
using YugiohDB.Models;

namespace YugiohDB 
{
    public class YugiohContext : DbContext 
    {
        DbSet<Card> Cards { get; set; }
        DbSet<CardPrice> CardPrices { get; set; }
        DbSet<CardSet> CardSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YugiohDatabase;Trusted_Connection=True;");
        }

    }
}