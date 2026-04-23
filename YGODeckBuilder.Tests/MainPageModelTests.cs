using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.Pages;

namespace YGODeckBuilder.Tests
{
    public class MainPageModelTests
    {
        [Fact]
        public async Task OnGet_EmptySearch_FallsBackToDarkMagician()
        {
            var provider = new Mock<ICardsProvider>();
            var cards = new List<Card> { new Card { Name = "Dark Magician" } };
            provider.Setup(p => p.GetSearchAsync("dark magician")).ReturnsAsync(cards);

            var model = new MainPageModel(provider.Object) { Search = "" };

            var result = await model.OnGet();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(model.Cards);
            Assert.Single(model.Cards);
            provider.Verify(p => p.GetSearchAsync("dark magician"), Times.Once);
        }

        [Fact]
        public async Task OnGet_WhitespaceSearch_FallsBackToDefault()
        {
            var provider = new Mock<ICardsProvider>();
            provider.Setup(p => p.GetSearchAsync("dark magician")).ReturnsAsync(new List<Card>());

            var model = new MainPageModel(provider.Object) { Search = "   " };

            await model.OnGet();

            provider.Verify(p => p.GetSearchAsync("dark magician"), Times.Once);
        }

        [Fact]
        public async Task OnGet_NullSearchResults_LeavesCardsNull()
        {
            var provider = new Mock<ICardsProvider>();
            provider.Setup(p => p.GetSearchAsync(It.IsAny<string>())).ReturnsAsync((ICollection<Card>)null);

            var model = new MainPageModel(provider.Object) { Search = "nothing" };

            await model.OnGet();

            Assert.Null(model.Cards);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 20)]
        [InlineData("2.5", 50)]
        public void GetCurrentChange_ConvertsWithTwentyMultiplier(string price, double expected)
        {
            Assert.Equal(expected, MainPageModel.GetCurrentChange(price));
        }

        [Fact]
        public void GetCurrentChange_ThrowsForNonNumeric()
        {
            Assert.Throws<FormatException>(() => MainPageModel.GetCurrentChange("abc"));
        }
    }
}
