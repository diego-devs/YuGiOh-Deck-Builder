using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using YGODeckBuilder.Pages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGODeckBuilder.Interfaces;
using Microsoft.DotNet.Scaffolding.Shared;

namespace YGODeckBuilder.Tests
{
    public class DeckBuilderTests
    {
        [Fact]
        public async Task OnGetAsync_DeckFileNameNotEmpty_DeckLoaded()
        {
            // Arrange
            var mockContext = new Mock<YgoContext>();
            var mockConfig = new Mock<IConfiguration>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockSession = new Mock<ISession>();
            var deck = new Deck { DeckName = "test" };
            var mockDeckUtility = new Mock<IDeckUtility>();
            var mockFileSystem = new Mock<IFileSystem>();
            var cards = new List<Card> { new Card { Name = "testCard" } };

            // Setup the DeckUtility mock
            mockDeckUtility.Setup(d => d.LoadDeckAsync(It.IsAny<string>())).ReturnsAsync(deck);

            // Setup the HttpContext and Session mock
            mockHttpContextAccessor.Setup(_ => _.HttpContext.Session).Returns(mockSession.Object);

            // Setup the FileSystem mock to return true, indicating the file exists
            mockFileSystem.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            // Setup the YgoContext mock to return a list of cards for the search
            mockContext.Setup(c => c.GetSearch(It.IsAny<string>())).Returns(cards);

            // Inject the mocks into the DeckBuilder instance
            var deckBuilder = new DeckBuilder(mockContext.Object, mockConfig.Object, mockHttpContextAccessor.Object, mockDeckUtility.Object, mockFileSystem.Object)
            {
                DeckFileName = "test.ydk"
            };

            // Act
            var result = await deckBuilder.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(deckBuilder.Deck);
        }
    }
}
