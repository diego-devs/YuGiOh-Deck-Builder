using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using YGOCardSearch.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YGOCardSearch.Data
{
    public class DeckUtility
    {
        private readonly IConfiguration _configuration;
        public Deck Deck { get; set; }
        // Database
        public readonly YgoContext Context;
        public DeckUtility(YgoContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            Context = db;
        }

        public List<DeckPreview> LoadDecksPreview()
        {
            string[] deckFiles = Directory.GetFiles(_configuration["Paths:DecksFolderPath"], "*.ydk");

            List<DeckPreview> deckPreviews = new List<DeckPreview>();

            foreach (var filePath in deckFiles)
            {
                try
                {
                    //var deck = LoadDeck(filePath); // Assuming LoadDeck is adjusted to not load from a single file
                    deckPreviews.Add(new DeckPreview { DeckName = Path.GetFileNameWithoutExtension(filePath)
                });
                    

                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                
            }
            return deckPreviews;
        }
       
        /// <summary>
        /// Loads a Deck from a .ydk file, extracting the main deck, extra deck, and side deck card lists.
        /// </summary>
        /// <param name="path">The file path of the .ydk file to load the Deck from.</param>
        /// <returns>The loaded Deck containing the main deck, extra deck, and side deck card lists.</returns>
        public async Task<Deck> LoadDeck(string path)
        {
            // Extract the deck name from the file name (excluding the extension)
            string deckName = Path.GetFileNameWithoutExtension(path);

            // Read all lines from the YDK deck file
            string[] deckLines = System.IO.File.ReadAllLines(path);

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
            List<Card> mainDeck = await GetCardListAsync(cleanedMainDeckIds);
            List<Card> extraDeck = await GetCardListAsync(cleanedExtraDeckIds);
            List<Card> sideDeck = await GetCardListAsync(cleanedSideDeckIds);

            // Create and populate the deck object
            Deck newDeck = new Deck()
            {
                MainDeck = mainDeck,
                ExtraDeck = extraDeck,
                SideDeck = sideDeck,
                DeckName = deckName
            };
            

            return newDeck;
        }

        private static string[] NormalizeSectionNotations(string[] deckLines)
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
        public async Task<List<Card>> GetCardListAsync(List<string> cardList)
        {
            var result = new List<Card>();

            foreach (var cardId in cardList)
            {
                if (int.TryParse(cardId, out int konamiCardId))
                {
                    var card = await Context.Cards.SingleOrDefaultAsync(c => c.KonamiCardId == konamiCardId);
                    if (card != null)
                    {
                        result.Add(card);
                    }
                }
            }

            return result;
        }
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


        public void PrepareCardData(Deck deck)
        {
            PrepareCardDataForDeck(deck.MainDeck);
            PrepareCardDataForDeck(deck.ExtraDeck);
            PrepareCardDataForDeck(deck.SideDeck);
        }

        private void PrepareCardDataForDeck(ICollection<Card> cards)
        {
            foreach (var card in cards)
            {
                card.CardImages = Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId).ToList();
                card.CardSets = Context.CardSets.Where(s => s.CardId == card.CardId).ToList();
                card.CardPrices = Context.CardPrices.Where(p => p.CardId == card.CardId).ToList();
            }
        }

        internal void PrepareCardDataSearch(List<Card> results)
        {
            foreach (var card in results)
            {
                card.CardImages = Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId).ToList();
                card.CardSets = Context.CardSets.Where(s => s.CardId == card.CardId).ToList();
                card.CardPrices = Context.CardPrices.Where(p => p.CardId == card.CardId).ToList();
            }
        }

        public async Task PrepareCardDataAsync(Deck deck)
        {
            await PrepareCardDataForDeckAsync(deck.MainDeck);
            await PrepareCardDataForDeckAsync(deck.ExtraDeck);
            await PrepareCardDataForDeckAsync(deck.SideDeck);
        }

        private async Task PrepareCardDataForDeckAsync(ICollection<Card> cards)
        {
            foreach (var card in cards)
            {
                card.CardImages = await Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId).ToListAsync();
                card.CardSets = await Context.CardSets.Where(s => s.CardId == card.CardId).ToListAsync();
                card.CardPrices = await Context.CardPrices.Where(p => p.CardId == card.CardId).ToListAsync();
            }
        }

        internal async Task PrepareCardDataSearchAsync(List<Card> results)
        {
            foreach (var card in results)
            {
                card.CardImages = await Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId).ToListAsync();
                card.CardSets = await Context.CardSets.Where(s => s.CardId == card.CardId).ToListAsync();
                card.CardPrices = await Context.CardPrices.Where(p => p.CardId == card.CardId).ToListAsync();
            }
        }


    }
}
