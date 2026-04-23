using System.Collections.Generic;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Tests
{
    public class DeckTests
    {
        [Fact]
        public void DefaultConstructor_InitializesEmptyCollections()
        {
            var deck = new Deck();
            Assert.Null(deck.DeckName);
            Assert.NotNull(deck.MainDeck);
            Assert.NotNull(deck.ExtraDeck);
            Assert.NotNull(deck.SideDeck);
            Assert.Equal(0, deck.TotalCards);
        }

        [Fact]
        public void NameConstructor_InitializesEmptyCollectionsAndName()
        {
            var deck = new Deck("My Deck");

            Assert.Equal("My Deck", deck.DeckName);
            Assert.NotNull(deck.MainDeck);
            Assert.NotNull(deck.ExtraDeck);
            Assert.NotNull(deck.SideDeck);
            Assert.Empty(deck.MainDeck);
            Assert.Empty(deck.ExtraDeck);
            Assert.Empty(deck.SideDeck);
            Assert.Equal(0, deck.TotalCards);
        }

        [Fact]
        public void FullConstructor_AssignsAllFields()
        {
            var main = new List<Card> { new Card(), new Card() };
            var extra = new List<Card> { new Card() };
            var side = new List<Card> { new Card(), new Card(), new Card() };

            var deck = new Deck("Deck", main, extra, side, "path/to/deck.ydk");

            Assert.Equal("Deck", deck.DeckName);
            Assert.Same(main, deck.MainDeck);
            Assert.Same(extra, deck.ExtraDeck);
            Assert.Same(side, deck.SideDeck);
            Assert.Equal("path/to/deck.ydk", deck.DeckFilePath);
        }

        [Fact]
        public void TotalCards_SumsAllThreeDecks()
        {
            var deck = new Deck("t")
            {
                MainDeck = new List<Card> { new Card(), new Card(), new Card() },
                ExtraDeck = new List<Card> { new Card() },
                SideDeck = new List<Card> { new Card(), new Card() }
            };

            Assert.Equal(6, deck.TotalCards);
        }
    }
}
