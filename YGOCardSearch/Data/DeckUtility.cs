using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using YGOCardSearch.Data.Models;
using Microsoft.Extensions.Configuration;

namespace YGOCardSearch.Data
{
    public class DeckUtility
    {
        private readonly IConfiguration _configuration;
        // Esto tambien debera cambiar por db:
        public List<Deck> LoadedDecks;
        // Deck a visualizar
        public Deck Deck { get; set; }
        // Database
        public readonly YgoContext Context;
        public DeckUtility(YgoContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            // Good place to initialize data
            // Load the Database from the Context class
            this.Context = db;


            LoadedDecks = new List<Deck>();
            Deck = new Deck();

            LoadedDecks.Add(LoadDeck());
            Deck = LoadedDecks.First(); // make selection with dropdrown menu 

            // PrepareDeck
            foreach (var card in Deck.MainDeck)
            {
                card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
            }
        }

        /// <summary>
        /// Cleans a list of card identifiers by removing non-digit characters and returns a new list containing only the digit values.
        /// </summary>
        /// <param name="deckList">The list of card identifiers to be cleaned.</param>
        /// <returns>A new list containing only the digit values from the card identifiers.</returns>
        public static List<string> CleanDeck(List<string> deckList)
        {
            var cleanedDeckList = new List<string>();

            foreach (var cardId in deckList)
            {
                var onlyDigits = new string(cardId.Where(char.IsDigit).ToArray());
                if (!string.IsNullOrEmpty(onlyDigits))
                {
                    cleanedDeckList.Add(onlyDigits);
                }
            }

            return cleanedDeckList;
        }
        /// <summary>
        /// Loads a Deck from a .ydk file, extracting the main deck, extra deck, and side deck card lists.
        /// </summary>
        /// <param name="path">The file path of the .ydk file to load the Deck from.</param>
        /// <returns>The loaded Deck containing the main deck, extra deck, and side deck card lists.</returns>
        public Deck LoadDeck()
        {
            // Get all .ydk files from the specified directory
            string[] deckFiles = Directory.GetFiles(_configuration["Paths:DecksFolderPath"], "*.ydk");

            // Choose the first .ydk file in the directory
            string deckFilePath = deckFiles.FirstOrDefault();

            // Check if a deck file exists
            if (string.IsNullOrEmpty(deckFilePath))
            {
                throw new FileNotFoundException("No deck (.ydk) files found in the specified directory.");
            }

            // Read all lines from the deck file
            string[] deckLines = System.IO.File.ReadAllLines(deckFilePath);

            // Find the indices of different sections in the deck file
            int mainIndex = Array.IndexOf(deckLines, "#main");
            int extraIndex = Array.IndexOf(deckLines, "#extra");
            int sideIndex = Array.IndexOf(deckLines, "!side");

            // Extract card IDs for each section
            List<string> mainDeckIds = deckLines.Skip(mainIndex + 1).Take(extraIndex - (mainIndex + 1)).ToList();
            List<string> extraDeckIds = deckLines.Skip(extraIndex + 1).Take(sideIndex - (extraIndex + 1)).ToList();
            List<string> sideDeckIds = deckLines.Skip(sideIndex + 1).ToList();

            // Clean the card IDs
            List<string> cleanedMainDeckIds = CleanDeck(mainDeckIds);
            List<string> cleanedExtraDeckIds = CleanDeck(extraDeckIds);
            List<string> cleanedSideDeckIds = CleanDeck(sideDeckIds);

            // Convert card IDs to card objects
            List<Card> mainDeck = GetCardList(cleanedMainDeckIds);
            List<Card> extraDeck = GetCardList(cleanedExtraDeckIds);
            List<Card> sideDeck = GetCardList(cleanedSideDeckIds);

            // Validate that at least one card is present in the main deck
            if (mainDeck.Count == 0)
            {
                throw new InvalidOperationException("The main deck must contain at least one card.");
            }

            // Create and populate the deck object
            Deck newDeck = new Deck();
            newDeck.MainDeck = mainDeck;
            newDeck.ExtraDeck = extraDeck;
            newDeck.SideDeck = sideDeck;
            newDeck.DeckName = newDeck.MainDeck.First().Name.ToString().ToLower();

            return newDeck;
        }

        /// <summary>
        /// Returns a list of CardModel objects based on a list of CardIdKonami by searching in YgoDB.
        /// </summary>
        /// <param name="cardList">The list of CardIdKonami to search for.</param>
        /// <returns>A list of CardModel objects.</returns>
        public List<Card> GetCardList(List<string> cardList)
        {
            var result = new List<Card>();

            foreach (var cardId in cardList)
            {
                if (int.TryParse(cardId, out int konamiCardId))
                {
                    var card = Context.Cards.FirstOrDefault(c => c.KonamiCardId == konamiCardId);
                    if (card != null)
                    {
                        result.Add(card);
                    }
                }
            }

            return result;
        }

       



        public void Sort()
        {
            Deck.MainDeck = Deck.MainDeck.OrderBy(c => c.Type).ToList();
            Deck.ExtraDeck = Deck.ExtraDeck.OrderBy(c => c.Type).ToList();
            Deck.SideDeck = Deck.SideDeck.OrderBy(c => c.Type).ToList();
        }
    }
}
