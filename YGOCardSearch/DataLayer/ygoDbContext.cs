using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Migrations;
using YGOCardSearch.Models;

namespace YGOCardSearch.DataLayer
{
    public class ygoDbContext : DbContext
    {
        public ygoDbContext(DbContextOptions<ygoDbContext> options) : base(options)
        {

        }
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<DeckModel> Decks { get; set; }
        public DbSet<CardImage> Images { get; set; }
        public DbSet<CardPrice> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<CardModel>().ToTable("Card");
            modelBuilder.Entity<CardImage>().ToTable("CardImage");
            modelBuilder.Entity<CardImage>().ToTable("CardPrice");
            modelBuilder.Entity<DeckModel>().ToTable("Deck");
        }
    }
}
