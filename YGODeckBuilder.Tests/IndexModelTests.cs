using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGODeckBuilder.Data.Models;
using YGODeckBuilder.Pages;

namespace YGODeckBuilder.Tests
{
    public class IndexModelTests
    {
        [Fact]
        public async Task OnGet_SearchIsNotNull_ReturnsCards()
        {
            // Arrange
            var mockCardsProvider = new Mock<ICardsProvider>();
            var cards = new List<Card> { new Card(), new Card() };
            mockCardsProvider.Setup(p => p.GetSearchAsync(It.IsAny<string>())).ReturnsAsync(cards);

            var indexModel = new MainPageModel(mockCardsProvider.Object)
            {
                Search = "test"
            };

            // Act
            var result = await indexModel.OnGet();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(indexModel.Cards);
        }

        [Fact]
        public void GetCurrentChange_ValidPrice_ReturnsCorrectChange()
        {
            // Act
            var result = MainPageModel.GetCurrentChange("10");

            // Assert
            Assert.Equal(200, result);
        }
    }
}
