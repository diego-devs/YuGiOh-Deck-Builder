using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using YGODeckBuilder.API;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder.Tests
{
    public class DecksManagerControllerTests : IDisposable
    {
        private readonly string _decksFolder;
        private readonly IConfiguration _config;
        private readonly YgoContext _context;
        private readonly Mock<IDeckUtility> _deckUtility;
        private readonly Mock<ILogger<DecksManagerController>> _logger;
        private readonly DecksManagerController _controller;

        public DecksManagerControllerTests()
        {
            _decksFolder = Path.Combine(Path.GetTempPath(), "ygo_decks_ctrl_" + Guid.NewGuid());
            Directory.CreateDirectory(_decksFolder);

            _config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Paths:DecksFolderPath"] = _decksFolder
            }).Build();

            var options = new DbContextOptionsBuilder<YgoContext>()
                .UseInMemoryDatabase("DecksMgrCtrl_" + Guid.NewGuid())
                .Options;
            _context = new YgoContext(options);

            _deckUtility = new Mock<IDeckUtility>();
            _logger = new Mock<ILogger<DecksManagerController>>();
            _controller = new DecksManagerController(_config, _context, _deckUtility.Object, _logger.Object);
        }

        public void Dispose()
        {
            _context.Dispose();
            if (Directory.Exists(_decksFolder))
                Directory.Delete(_decksFolder, recursive: true);
        }

        [Fact]
        public void SaveDeck_InvalidName_ReturnsBadRequest()
        {
            var deck = new Deck("../bad") { MainDeck = new List<Card>() };
            var result = _controller.SaveDeck(deck);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void SaveDeck_NullMainDeck_ReturnsStatus500()
        {
            var deck = new Deck("good_name") { MainDeck = null };
            var result = _controller.SaveDeck(deck);
            var status = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, status.StatusCode);
        }

        [Fact]
        public void SaveDeck_Valid_CallsExportAndRecordsInDb()
        {
            var deck = new Deck("valid_deck") { MainDeck = new List<Card>() };

            var result = _controller.SaveDeck(deck);

            Assert.IsType<OkResult>(result);
            _deckUtility.Verify(d => d.ExportDeck(It.Is<Deck>(x => x.DeckName == "valid_deck")), Times.Once);
            Assert.Single(_context.Decks);
        }

        [Fact]
        public void DuplicateDeck_InvalidName_ReturnsBadRequest()
        {
            var result = _controller.DuplicateDeck("../evil");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DuplicateDeck_ExistingTarget_ReturnsConflict()
        {
            File.WriteAllText(Path.Combine(_decksFolder, "source.ydk"), "");
            File.WriteAllText(Path.Combine(_decksFolder, "Copy_source.ydk"), "");

            var result = _controller.DuplicateDeck("source");

            Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public void DuplicateDeck_Valid_CreatesCopy()
        {
            File.WriteAllText(Path.Combine(_decksFolder, "source.ydk"), "#main\n#extra\n!side\n");

            var result = _controller.DuplicateDeck("source");

            Assert.IsType<OkResult>(result);
            Assert.True(File.Exists(Path.Combine(_decksFolder, "Copy_source.ydk")));
        }

        [Fact]
        public void DeleteDeck_InvalidName_ReturnsBadRequest()
        {
            var result = _controller.DeleteDeck("../evil");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeleteDeck_Valid_RemovesFile()
        {
            var path = Path.Combine(_decksFolder, "doomed.ydk");
            File.WriteAllText(path, "");

            var result = _controller.DeleteDeck("doomed");

            Assert.IsType<OkResult>(result);
            Assert.False(File.Exists(path));
        }

        [Fact]
        public void RenameDeck_InvalidNewName_ReturnsBadRequest()
        {
            var result = _controller.RenameDeck(new RenameDeckRequest { OldDeckName = "ok", NewDeckName = "../bad" });
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void RenameDeck_Valid_MovesFile()
        {
            var oldPath = Path.Combine(_decksFolder, "oldname.ydk");
            File.WriteAllText(oldPath, "");

            var result = _controller.RenameDeck(new RenameDeckRequest
            {
                OldDeckName = "oldname",
                NewDeckName = "newname"
            });

            Assert.IsType<OkResult>(result);
            Assert.False(File.Exists(oldPath));
            Assert.True(File.Exists(Path.Combine(_decksFolder, "newname.ydk")));
        }

        [Fact]
        public void CreateDeck_InvalidName_ReturnsBadRequest()
        {
            var result = _controller.CreateDeck("../evil");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateDeck_Valid_CallsExportAndRedirects()
        {
            var result = _controller.CreateDeck("fresh_deck");

            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/DeckBuilder", redirect.PageName);
            _deckUtility.Verify(d => d.ExportDeck(It.Is<Deck>(x => x.DeckName == "fresh_deck")), Times.Once);
        }
    }
}
