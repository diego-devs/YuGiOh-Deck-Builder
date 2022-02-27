using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using YGOCardSearch.Models;
using YGOCardSearch.DataProviders;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace YGOCardSearch.Pages
{
    public class DeckBuilder : PageModel
    {
        // Deck a visualizar
        public DeckModel Deck { get; set; }

        // Proveedor de datos, archivo de texto
        public List<CardModel> repo { get; set; }

        public async Task<IActionResult> OnGet()
        {
            // Load all cards from local repo? 
            repo = LoadCardsLocalRepo();

            //  Load a local deck file
            string path = @"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\Decks\deck1.ydk";
            Deck = LoadDeck(path);
            return Page();

        }
        public List<CardModel> LoadCardsLocalRepo() 
        {
            // Buen lugar para cargar los datos
            var allCardsPath = @"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\data\allCards.txt";
            var jsonCards = System.IO.File.ReadAllText(allCardsPath);
            var AllCards = JsonSerializer.Deserialize<List<CardModel>>(jsonCards);
            return AllCards;
        }
        public DeckModel LoadDeck(string path)
        {
            string[] ydkDeck = System.IO.File.ReadAllLines(path);
            List<string> deckIds = new List<string>(ydkDeck);

            List<int> mainDeckIds = new List<int>();
            List<int> extraDeckIds = new List<int>();
            List<int> sideDecksIds = new List<int>();

            int mainIndex = deckIds.FindIndex(c => c.Contains("#main"));
            int extraIndex = deckIds.FindIndex(r => r.Contains("#extra"));
            int sideIndex = deckIds.FindIndex(c => c.Contains("!side"));

            // Ids from main deck
            var mainDeckResult = deckIds.Skip(mainIndex + 1).Take(extraIndex - (mainIndex + 1));
            foreach (var cardId in mainDeckResult)
            {
                mainDeckIds.Add(Convert.ToInt32(cardId));
            }
            // Ids from extra deck
            var extraDeckResult = deckIds.Skip(extraIndex + 1).Take(sideIndex - (extraIndex + 1));
            foreach (var cardId in extraDeckResult)
            {
                extraDeckIds.Add(Convert.ToInt32(cardId));
            }
            // Ids from side deck
            var sideDeckResult = deckIds.Skip(sideIndex + 1).Take(sideIndex - (extraIndex + 1));
            foreach (var cardId in sideDeckResult)
            {
                sideDecksIds.Add(Convert.ToInt32(cardId));
            }
            // Get all cards one by one 
            var mainDeck = new List<CardModel>();
            var extraDeck = new List<CardModel>();
            var sideDeck = new List<CardModel>();

            foreach (var id in mainDeckIds)
            {
                var card = repo.Where(c => c.Id == id);
                mainDeck.AddRange(card);
            }

            foreach (var id in extraDeckIds)
            {
                var card = repo.Where(c => c.Id == id);
                extraDeck.AddRange(card);
            }
            foreach (var id in sideDecksIds)
            {
                var card = repo.Where(c => c.Id == id);
                sideDeck.AddRange(card);
            }

            // Create deck with cards retrieved
            DeckModel newDeck = new DeckModel();

            newDeck.MainDeck = mainDeck;
            newDeck.ExtraDeck = extraDeck;
            newDeck.SideDeck = sideDeck;
            newDeck.DeckName = mainDeck[0].Name.ToLower() + "_deck";
            return newDeck;

        }
        
    }
}
