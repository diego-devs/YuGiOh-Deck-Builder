using Microsoft.EntityFrameworkCore;
using YGOCardSearch.Data.Models;
using YGOCardSearch.Models;

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