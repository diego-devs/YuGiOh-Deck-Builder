using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using YGOCardSearch.Data.Models;
using YGOCardSearch.DataProviders;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using YGOCardSearch.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using Newtonsoft.Json;

namespace YGOCardSearch.Pages
{
    public class DeckBuilder : PageModel
    {
        
        private readonly IConfiguration _configuration;
        // Esto tambien debera cambiar por db:
        public List<Deck> LoadedDecks; // no need of this since DecksManager page will be a thing
        // Deck a visualizar
        public Deck Deck { get; set; }
        public string currentDeckName { get; set; }
        // Database
        public readonly YgoContext Context;
        [BindProperty(SupportsGet = true)]
        public string searchQuery { get; set; } 
        public List<Card> SearchCards { get; set; }
        public string decksPath { get; set; } 

        // Dependency injection of both Configuration and YgoContext 
        public DeckBuilder(YgoContext db, IConfiguration configuration)
        {
            // Good place to initialize data
            // Load the Database
            this.Context = db;
            this._configuration = configuration;
            this.decksPath = _configuration["Paths:DecksFolderPath"];

            LoadedDecks = new List<Deck>();
            Deck = new Deck();

            // Load all decks as a list of decks
            LoadedDecks.Add(LoadDeck(decksPath));

            // Get the selected Deck as focus deck
            Deck = LoadedDecks.FirstOrDefault(); // developer todo: make selection with dropdrown menu 
            currentDeckName = Deck.DeckName;

            // Prepare card images, sets and prices from all decks:
            foreach (var card in Deck.MainDeck)
            {
                card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
            }
            foreach (var card in Deck.ExtraDeck)
            {
                card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
            }
            foreach (var card in Deck.SideDeck)
            {
                card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
            }
            
        }
        public IActionResult OnGet()
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var results = Context.GetSearch(searchQuery);
                if (results != null)
                {
                    // Prepare card infos
                    foreach (var card in results)
                    {
                        card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                        card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                        card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
                    }
                    SearchCards = new List<Card>(results);
                    
                }
            }
            else
            {
                var results = Context.GetSearch("dark ruler");
                if (results != null)
                {
                    // Prepare card infos
                    foreach (var card in results)
                    {
                        card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                        card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                        card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
                    }
                    SearchCards = new List<Card>(results);
                }
                else if (results == null)
                {
                    Console.WriteLine("No cards matching the search");
                }; 
            }
            return Page();
        }
        public void SearchForCards()
        {
            var results = Context.GetSearch(searchQuery);
            if (results != null)
            {
                // Prepare card infos
                foreach (var card in results)
                {
                    card.CardImages = new List<CardImages>(Context.CardImages.Where(i => i.CardImageId == card.KonamiCardId));
                    card.CardSets = new List<CardSet>(Context.CardSets.Where(s => s.CardId == card.CardId));
                    card.CardPrices = new List<CardPrices>(Context.CardPrices.Where(p => p.CardId == card.CardId));
                }
                SearchCards = new List<Card>(results);

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
        //public List<Card> GetCardList(List<string> cardList) 
        //{
        //        var cards = new List<Card>();
        //        foreach (var cardId in cardList)
        //        {
        //            if (Context.Cards.Exists(c => c.KonamiCardId == Convert.ToInt32(cardId)))
        //            {
        //                Card card = AllCards.Single(c => c.KonamiCardId == Convert.ToInt32(cardId));
        //                cards.Add(card);
        //            }
        //        }
        //    return cards;
        //}
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

        
        /// <summary>
        /// Recursively updates the "CardImages" property of a list of Card objects.
        /// </summary>
        /// <param name="cardList">The list of Card objects to update.</param>
        /// <param name="propValue">The new value for the "CardImages" property.</param>
        void UpdateImagesProperty(List<Card> cardList, CardImages propValue)
        {
            if (cardList.Count == 0)
                return;

            // change the property of the current object
            cardList[0].GetType().GetProperty("CardImages").SetValue(cardList[0], propValue);

            // call the function again with the tail of the list
            UpdateImagesProperty(cardList.Skip(1).ToList(), propValue);
        }

    }
}
