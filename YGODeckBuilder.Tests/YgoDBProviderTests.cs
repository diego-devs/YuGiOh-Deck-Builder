using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.DataProviders;

namespace YGODeckBuilder.Tests
{
    public class YgoDBProviderTests : IDisposable
    {
        private readonly YgoContext _context;
        private readonly YgoDBProvider _provider;

        public YgoDBProviderTests()
        {
            var options = new DbContextOptionsBuilder<YgoContext>()
                .UseInMemoryDatabase("YgoDBProvider_" + Guid.NewGuid())
                .Options;
            _context = new YgoContext(options);
            _provider = new YgoDBProvider(_context);
        }

        public void Dispose() => _context.Dispose();

        [Fact]
        public async Task GetAllCardsAsync_ReturnsEveryCard()
        {
            _context.Cards.AddRange(
                new Card { CardId = 1, KonamiCardId = 10, Name = "A", Desc = "" },
                new Card { CardId = 2, KonamiCardId = 20, Name = "B", Desc = "" });
            await _context.SaveChangesAsync();

            var all = await _provider.GetAllCardsAsync();
            Assert.Equal(2, all.Count);
        }

        [Fact]
        public async Task GetCardAsync_ReturnsCardByKonamiId()
        {
            _context.Cards.Add(new Card { CardId = 1, KonamiCardId = 1234, Name = "Magician", Desc = "" });
            await _context.SaveChangesAsync();

            var card = await _provider.GetCardAsync(1234);

            Assert.NotNull(card);
            Assert.Equal("Magician", card.Name);
        }

        [Fact]
        public async Task GetSearchAsync_MatchesByName()
        {
            _context.Cards.AddRange(
                new Card { CardId = 1, KonamiCardId = 1, Name = "Blue-Eyes White Dragon", Desc = "strong" },
                new Card { CardId = 2, KonamiCardId = 2, Name = "Dark Magician", Desc = "wizard" });
            await _context.SaveChangesAsync();

            var results = await _provider.GetSearchAsync("blue-eyes");

            Assert.Single(results);
            Assert.Equal("Blue-Eyes White Dragon", results.First().Name);
        }

        [Fact]
        public async Task GetSearchAsync_MatchesByDescription()
        {
            _context.Cards.Add(new Card { CardId = 1, KonamiCardId = 1, Name = "X", Desc = "This card returns from graveyard" });
            await _context.SaveChangesAsync();

            var results = await _provider.GetSearchAsync("graveyard");

            Assert.Single(results);
        }

        [Fact]
        public async Task GetSearchAsync_NoMatch_ReturnsEmpty()
        {
            _context.Cards.Add(new Card { CardId = 1, KonamiCardId = 1, Name = "X", Desc = "Y" });
            await _context.SaveChangesAsync();

            var results = await _provider.GetSearchAsync("nothing_matches");

            Assert.Empty(results);
        }
    }
}
