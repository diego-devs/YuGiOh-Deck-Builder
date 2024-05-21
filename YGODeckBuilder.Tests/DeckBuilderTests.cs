using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGODeckBuilder.Data.Models;
using YGODeckBuilder.Data;
using YGODeckBuilder.Pages;
using Microsoft.Extensions.Configuration;

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
            var deck = new Deck { DeckName = "test" };
            var deckUtility = new Mock<DeckUtility>(mockContext.Object, mockConfig.Object);
            deckUtility.Setup(d => d.LoadDeckAsync(It.IsAny<string>())).ReturnsAsync(deck);

            var deckBuilder = new DeckBuilder(mockContext.Object, mockConfig.Object, mockHttpContextAccessor.Object)
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
