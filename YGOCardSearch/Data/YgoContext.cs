using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Migrations;
using YGOCardSearch.Models;

namespace YGOCardSearch.DataLayer
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
        public DbSet<DeckModel> Decks { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)     
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Trusted_Connection=True;");
        }

    }
}
