using Microsoft.EntityFrameworkCore;
using YugiohDB.Models;

namespace YugiohDB
{
    public class YugiohContext : DbContext 
    {
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<PriceModel> CardPrices { get; set; }
        public DbSet<SetModel> CardSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Trusted_Connection=True;");
        }

    }
}