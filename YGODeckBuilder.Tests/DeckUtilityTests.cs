using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Tests
{
    public class DeckUtilityTests : IDisposable
    {
        private readonly string _tempFolder;
        private readonly IConfiguration _config;
        private readonly YgoContext _context;

        public DeckUtilityTests()
        {
            _tempFolder = Path.Combine(Path.GetTempPath(), "ygodeck_tests_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempFolder);

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Paths:DecksFolderPath"] = _tempFolder
                })
                .Build();

            var options = new DbContextOptionsBuilder<YgoContext>()
                .UseInMemoryDatabase("DeckUtilityTests_" + Guid.NewGuid())
                .Options;
            _context = new YgoContext(options);
        }

        public void Dispose()
        {
            _context.Dispose();
            if (Directory.Exists(_tempFolder))
                Directory.Delete(_tempFolder, recursive: true);
        }

        [Fact]
        public void Constructor_ThrowsWhenDecksFolderPathMissing()
        {
            var emptyConfig = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            Assert.Throws<ArgumentNullException>(() => new DeckUtility(_context, emptyConfig));
        }

        [Theory]
        [InlineData("My Deck", "My Deck")]
        [InlineData("valid-name_1", "valid-name_1")]
        [InlineData("abc123", "abc123")]
        public void SanitizeDeckName_AllowsValidNames(string input, string expected)
        {
            Assert.Equal(expected, DeckUtility.SanitizeDeckName(input));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("../evil")]
        [InlineData("bad/name")]
        [InlineData("bad\\name")]
        [InlineData("name$")]
        [InlineData("name!")]
        public void SanitizeDeckName_RejectsInvalidNames(string input)
        {
            Assert.Null(DeckUtility.SanitizeDeckName(input));
        }

        [Fact]
        public void SanitizeDeckName_RejectsTooLongName()
        {
            var name = new string('a', 101);
            Assert.Null(DeckUtility.SanitizeDeckName(name));
        }

        [Fact]
        public void SanitizeDeckName_AcceptsMaxLengthName()
        {
            var name = new string('a', 100);
            Assert.Equal(name, DeckUtility.SanitizeDeckName(name));
        }

        [Fact]
        public void IsPathSafe_TrueForFileInsideBase()
        {
            var inside = Path.Combine(_tempFolder, "deck.ydk");
            Assert.True(DeckUtility.IsPathSafe(_tempFolder, inside));
        }

        [Fact]
        public void IsPathSafe_FalseForPathOutsideBase()
        {
            var outside = Path.Combine(Path.GetTempPath(), "other_folder", "deck.ydk");
            Assert.False(DeckUtility.IsPathSafe(_tempFolder, outside));
        }

        [Fact]
        public void IsPathSafe_FalseForTraversal()
        {
            var evil = Path.Combine(_tempFolder, "..", "..", "evil.ydk");
            Assert.False(DeckUtility.IsPathSafe(_tempFolder, evil));
        }

        [Theory]
        [InlineData(new[] { "12345", "67890" }, new[] { "12345", "67890" })]
        [InlineData(new[] { "12345 ", "abc123" }, new[] { "12345", "123" })]
        [InlineData(new[] { "", "xyz" }, new string[0])]
        public void CleanDeck_StripsNonDigitsAndEmptyEntries(string[] input, string[] expected)
        {
            var result = DeckUtility.CleanDeck(input.ToList());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LoadDecksPreview_ReturnsEntryPerYdkFile()
        {
            File.WriteAllText(Path.Combine(_tempFolder, "deck1.ydk"), "#main\n#extra\n!side\n");
            File.WriteAllText(Path.Combine(_tempFolder, "deck2.ydk"), "#main\n#extra\n!side\n");
            File.WriteAllText(Path.Combine(_tempFolder, "notes.txt"), "ignore me");

            var util = new DeckUtility(_context, _config);

            var previews = util.LoadDecksPreview();

            Assert.Equal(2, previews.Count);
            Assert.Contains(previews, p => p.DeckName == "deck1");
            Assert.Contains(previews, p => p.DeckName == "deck2");
        }

        [Fact]
        public void ExportDeck_WritesAllSectionsAndIds()
        {
            var util = new DeckUtility(_context, _config);
            var deck = new Deck("my_deck")
            {
                MainDeck = new List<Card> { new Card { KonamiCardId = 1 }, new Card { KonamiCardId = 2 } },
                ExtraDeck = new List<Card> { new Card { KonamiCardId = 3 } },
                SideDeck = new List<Card> { new Card { KonamiCardId = 4 } }
            };

            util.ExportDeck(deck);

            var filePath = Path.Combine(_tempFolder, "my_deck.ydk");
            Assert.True(File.Exists(filePath));
            var lines = File.ReadAllLines(filePath);
            Assert.Contains("#main", lines);
            Assert.Contains("#extra", lines);
            Assert.Contains("!side", lines);
            Assert.Contains("1", lines);
            Assert.Contains("2", lines);
            Assert.Contains("3", lines);
            Assert.Contains("4", lines);
        }

        [Fact]
        public async Task LoadDeckAsync_ParsesYdkSectionsAndCards()
        {
            _context.Cards.AddRange(
                new Card { CardId = 101, KonamiCardId = 1001, Name = "A", Desc = "" },
                new Card { CardId = 102, KonamiCardId = 1002, Name = "B", Desc = "" },
                new Card { CardId = 103, KonamiCardId = 1003, Name = "C", Desc = "" }
            );
            await _context.SaveChangesAsync();

            var filePath = Path.Combine(_tempFolder, "Test.ydk");
            File.WriteAllLines(filePath, new[]
            {
                "#main",
                "1001",
                "1002",
                "#extra",
                "1003",
                "!side"
            });

            var util = new DeckUtility(_context, _config);
            var deck = await util.LoadDeckAsync(filePath);

            Assert.Equal("Test", deck.DeckName);
            Assert.Equal(2, deck.MainDeck.Count);
            Assert.Single(deck.ExtraDeck);
            Assert.Empty(deck.SideDeck);
        }

        [Fact]
        public async Task LoadDeckAsync_NormalizesCasedAndWhitespacedSections()
        {
            _context.Cards.Add(new Card { CardId = 1, KonamiCardId = 42, Name = "X", Desc = "" });
            await _context.SaveChangesAsync();

            var filePath = Path.Combine(_tempFolder, "Weird.ydk");
            File.WriteAllLines(filePath, new[]
            {
                "  #MAIN",
                "42",
                "#Extra",
                "!Side"
            });

            var util = new DeckUtility(_context, _config);
            var deck = await util.LoadDeckAsync(filePath);

            Assert.Single(deck.MainDeck);
            Assert.Empty(deck.ExtraDeck);
            Assert.Empty(deck.SideDeck);
        }

        [Fact]
        public async Task GetCardListAsync_SkipsUnknownAndNonNumericIds()
        {
            _context.Cards.Add(new Card { CardId = 1, KonamiCardId = 555, Name = "X", Desc = "" });
            await _context.SaveChangesAsync();

            var util = new DeckUtility(_context, _config);
            var result = await util.GetCardListAsync(new List<string> { "555", "999", "not-a-number" });

            Assert.Single(result);
            Assert.Equal(555, result[0].KonamiCardId);
        }

        [Fact]
        public void DrawCards_RemovesDrawnCardsFromMainDeck()
        {
            var util = new DeckUtility(_context, _config);
            util.Deck = new Deck("d")
            {
                MainDeck = new List<Card>
                {
                    new Card { KonamiCardId = 1 },
                    new Card { KonamiCardId = 2 },
                    new Card { KonamiCardId = 3 }
                }
            };

            var drawn = util.DrawCards(2);

            Assert.Equal(2, drawn.Count);
            Assert.Single(util.Deck.MainDeck);
        }

        [Fact]
        public void DrawCards_DrawingOne_RemovesFirstCard()
        {
            var util = new DeckUtility(_context, _config);
            var first = new Card { KonamiCardId = 1 };
            util.Deck = new Deck("d")
            {
                MainDeck = new List<Card>
                {
                    first,
                    new Card { KonamiCardId = 2 },
                    new Card { KonamiCardId = 3 }
                }
            };

            var drawn = util.DrawCards(1);

            Assert.Single(drawn);
            Assert.Same(first, drawn[0]);
            Assert.Equal(2, util.Deck.MainDeck.Count);
            Assert.DoesNotContain(first, util.Deck.MainDeck);
        }

        [Fact]
        public void ShuffleDeck_PreservesAllCards()
        {
            var util = new DeckUtility(_context, _config);
            var cards = Enumerable.Range(1, 40).Select(i => new Card { KonamiCardId = i }).ToList();
            util.Deck = new Deck("d") { MainDeck = new List<Card>(cards) };

            util.ShuffleDeck();

            Assert.Equal(cards.Count, util.Deck.MainDeck.Count);
            foreach (var c in cards)
                Assert.Contains(c, util.Deck.MainDeck);
        }
    }
}
