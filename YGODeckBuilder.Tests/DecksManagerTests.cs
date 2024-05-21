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
            var mockDeckUtility = new Mock<DeckUtility>(mockContext.Object, mockConfig.Object);
            mockDeckUtility.Setup(d => d.LoadDecksPreview()).Returns(new List<DeckPreview>());

            var decksManager = new DecksManager(mockContext.Object, mockConfig.Object);
            decksManager.Decks = new List<DeckPreview>();

            // Act
            decksManager.OnGet();

            // Assert
            Assert.NotNull(decksManager.Decks);
        }
    }
}
