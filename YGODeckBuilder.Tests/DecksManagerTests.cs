using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.Models;
using YGODeckBuilder.Pages;
using Xunit;
using Microsoft.Extensions.Configuration;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder.Tests
{
    public class DecksManagerTests
    {
        [Fact]
        public void OnGet_LoadsDecksPreview()
        {
            // Arrange
            var mockContext = new Mock<YgoContext>();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Paths:DecksFolderPath"]).Returns("someValidPath");

            var mockDeckUtility = new Mock<IDeckUtility>();
            var deckPreviews = new List<DeckPreview> { new DeckPreview { DeckName = "testDeck" } };
            mockDeckUtility.Setup(d => d.LoadDecksPreview()).Returns(deckPreviews);

            var decksManager = new DecksManager(mockContext.Object, mockConfig.Object, mockDeckUtility.Object);

            // Act
            decksManager.OnGet();

            // Assert
            Assert.NotNull(decksManager.Decks);
            Assert.Equal(deckPreviews, decksManager.Decks);
        }
    }
}
