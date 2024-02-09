using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YGOCardSearch.Data;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Pages
{
    public class DecksManagerModel : PageModel
    {
        private readonly IConfiguration _configuration;

        // Database
        public readonly YgoContext Context;

        public DecksManagerModel(YgoContext context, IConfiguration configuration)
        {
            // Load the Database
            this.Context = context;
            _configuration = configuration;
        }

        public List<DeckView> Decks { get; set; } = new List<DeckView>();


        public void OnGet()
        {
            LoadDecks();
        }

        public void LoadDecks()
        {
            var decksFolderPath = _configuration.GetValue<string>("Paths:DecksFolderPath");
            string[] deckFiles = Directory.GetFiles(decksFolderPath, "*.ydk");

            foreach (var filePath in deckFiles)
            {
                try
                {
                    var deck = LoadDeck(filePath); // Assuming LoadDeck is adjusted to not load from a single file
                    Decks.Add(new DeckView { DeckId = deck.DeckId, DeckName = deck.DeckName });
                }
                catch (FileNotFoundException ex)
                {
                    // Handle exceptions or log errors as needed
                }
            }
        }

        /// <summary>
        /// Loads a Deck from a .ydk file, extracting the main deck, extra deck, and side deck card lists.
        /// </summary>
        /// <param name="path">The file path of the .ydk file to load the Deck from.</param>
        /// <returns>The loaded Deck containing the main deck, extra deck, and side deck card lists.</returns>
        public Deck LoadDeck(string path)
        {
            // Get all .ydk files from the specified directory
            string[] deckFiles = Directory.GetFiles(path, "*.ydk");

            // Choose the first .ydk file in the directory
            string deckFilePath = deckFiles.FirstOrDefault();

            // Check if a deck file exists
            if (string.IsNullOrEmpty(deckFilePath))
            {
                throw new FileNotFoundException("No deck (.ydk) files found in the specified directory.");
            }

            // Extract the deck name from the file name (excluding the extension)
            string deckName = Path.GetFileNameWithoutExtension(deckFilePath);

            // Read all lines from the deck file
            string[] deckLines = System.IO.File.ReadAllLines(deckFilePath);

            // Normalize the section notations
            deckLines = NormalizeSectionNotations(deckLines);

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

            // Create and populate the deck object
            Deck newDeck = new Deck();
            newDeck.MainDeck = mainDeck;
            newDeck.ExtraDeck = extraDeck;
            newDeck.SideDeck = sideDeck;
            newDeck.DeckName = deckName;

            return newDeck;
        }

        private string[] NormalizeSectionNotations(string[] deckLines)
        {
            for (int i = 0; i < deckLines.Length; i++)
            {
                deckLines[i] = deckLines[i].Trim(); // Remove leading and trailing whitespaces
                if (deckLines[i].StartsWith("#main", StringComparison.OrdinalIgnoreCase))
                {
                    deckLines[i] = "#main";
                }
                else if (deckLines[i].StartsWith("#extra", StringComparison.OrdinalIgnoreCase))
                {
                    deckLines[i] = "#extra";
                }
                else if (deckLines[i].StartsWith("!side", StringComparison.OrdinalIgnoreCase))
                {
                    deckLines[i] = "!side";
                }
            }

            return deckLines;
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

    }
}
