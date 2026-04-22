using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YGODeckBuilder.API;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Tests
{
    public class CardsControllerTests : IDisposable
    {
        private readonly YgoContext _context;
        private readonly CardsController _controller;

        public CardsControllerTests()
        {
            var options = new DbContextOptionsBuilder<YgoContext>()
                .UseInMemoryDatabase("CardsCtrl_" + Guid.NewGuid())
                .Options;
            _context = new YgoContext(options);

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            _controller = new CardsController(config, _context);
        }

        public void Dispose() => _context.Dispose();

        [Fact]
        public async Task GetCardAsync_ExistingId_ReturnsOk()
        {
            _context.Cards.Add(new Card { CardId = 1, Name = "A", Desc = "" });
            await _context.SaveChangesAsync();

            var result = await _controller.GetCardAsync(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var card = Assert.IsType<Card>(ok.Value);
            Assert.Equal("A", card.Name);
        }

        [Fact]
        public async Task GetCardAsync_MissingId_ReturnsNotFound()
        {
            var result = await _controller.GetCardAsync(42);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCardsAsync_NoFilter_ReturnsAll()
        {
            _context.Cards.AddRange(
                new Card { CardId = 1, Name = "A", Desc = "" },
                new Card { CardId = 2, Name = "B", Desc = "" });
            await _context.SaveChangesAsync();

            var result = await _controller.GetCardsAsync();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<Card>>(ok.Value);
            Assert.Equal(2, new List<Card>(list).Count);
        }

        [Fact]
        public async Task GetCardsAsync_IdFilter_ReturnsOnlyMatching()
        {
            _context.Cards.AddRange(
                new Card { CardId = 1, Name = "A", Desc = "" },
                new Card { CardId = 2, Name = "B", Desc = "" });
            await _context.SaveChangesAsync();

            var result = await _controller.GetCardsAsync(2);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = new List<Card>((IEnumerable<Card>)ok.Value);
            Assert.Single(list);
            Assert.Equal("B", list[0].Name);
        }

        [Fact]
        public async Task GetCardsAsync_EmptyDb_ReturnsNotFound()
        {
            var result = await _controller.GetCardsAsync();
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
