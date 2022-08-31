using Microsoft.EntityFrameworkCore;
using YugiohDB.Models;

namespace YugiohDB
{
    public class YugiohContext : DbContext 
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<CardSet> CardSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Trusted_Connection=True;");
        }

    }
}