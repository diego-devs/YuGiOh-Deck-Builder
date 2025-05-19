using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System;
using YGODeckBuilder.Data.EntityModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace YGODeckBuilder.Data
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
        public DbSet<CardPrices> CardPrices { get; set; }
        public DbSet<CardSet> CardSets { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<CardImages> CardImages { get; set; }
        public DbSet<SetInfo> SetsInfo { get; set; }
        public DbSet<BanlistInfo> CardsBanlist { get; set; }
        public DbSet<MiscInfo> MiscInfos { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionCard> CollectionCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)     
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=YgoDB;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;");
        }
        public Card GetCard(int id)
        {
            return Cards.ElementAt(id);
        }
        public virtual List<Card> GetSearch(string searchQuery)
        {
            string normalizedQuery = searchQuery.ToLower();
            normalizedQuery.Normalize();

            // Use LINQ to filter cards that match the search query
            var matchingCards = Cards
                .Where(card =>
                    card.Name.ToLower().Contains(normalizedQuery) ||
                    card.Desc.ToLower().Contains(normalizedQuery))
                .ToList();
            if (matchingCards.IsNullOrEmpty())
            {
                Console.WriteLine("No card found.");
                
            }

            return matchingCards;
        }
        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is Collection && x.State == EntityState.Modified);
            foreach (var entry in modifiedEntries)
            {
                ((Collection)entry.Entity).LastModifiedDate = DateTime.UtcNow;
            }
            return base.SaveChanges();
        }

    }
}
