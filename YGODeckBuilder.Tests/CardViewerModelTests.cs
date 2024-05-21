using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGODeckBuilder.Data.Models;
using YGODeckBuilder.Pages;
using Moq;

namespace YGODeckBuilder.Tests
{
    public class CardViewerModelTests
    {
        [Fact]
        public async Task OnGet_CardExists_ReturnsPage()
        {
            // Arrange
            var mockCardsProvider = new Mock<ICardsProvider>();
            var mockConfig = new Mock<IConfiguration>();
            var card = new Card();
            mockCardsProvider.Setup(p => p.GetCardAsync(It.IsAny<int>())).ReturnsAsync(card);

            var cardViewerModel = new CardViewerModel(mockCardsProvider.Object, mockConfig.Object);

            // Act
            var result = await cardViewerModel.OnGet(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(cardViewerModel.Card);
        }

        [Fact]
        public async Task OnGet_CardDoesNotExist_RedirectsToIndex()
        {
            // Arrange
            var mockCardsProvider = new Mock<ICardsProvider>();
            var mockConfig = new Mock<IConfiguration>();
            mockCardsProvider.Setup(p => p.GetCardAsync(It.IsAny<int>())).ReturnsAsync((Card)null);

            var cardViewerModel = new CardViewerModel(mockCardsProvider.Object, mockConfig.Object);

            // Act
            var result = await cardViewerModel.OnGet(1);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
