using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using YGOCardSearch.Models;
using System.IO;
using System.Linq;

namespace YGOCardSearch.Pages
{
    public class DeckBuilder : PageModel
    {
        public DeckModel Deck { get; set; }
        public void OnGet()
        {
            Deck = new DeckModel();
            Deck.DeckName = "deckfromcode";

            //  Load a test deck
            string path = @"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\Decks\deck2.ydk";

            string[] deckCardsIds = System.IO.File.ReadAllLines(path);
            List<string> deckIds = new List<string>(deckCardsIds);

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
        }

        public void AddCard(CardModel card) 
        {
            Deck.MainDeck.Add(card);
        }
    }
}
