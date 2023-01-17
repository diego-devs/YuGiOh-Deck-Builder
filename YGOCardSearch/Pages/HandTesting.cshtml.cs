using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using YGOCardSearch.Data;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Pages
{

    public class HandTestingModel : PageModel
    {
        public Deck Deck { get; set; }
        public List<Card> MainDeck { get; set; }
        public List<Card> Hand {get;set;}


        public async Task<IActionResult> OnGet(Deck deck)
        {
            Deck = deck;
            MainDeck = new List<Card>(this.Deck.MainDeck);
            Hand = DrawCards();
            if (Hand != null)
            {
                return Page();
            }
            return RedirectToPage("DeckBuilder");
        }
        public List<Card> DrawCards()
        {
            var r = new Random();
            var cards = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                cards.Add(MainDeck[r.Next(0, MainDeck.Count)]);
            }
            return cards;
        }
    }
}
