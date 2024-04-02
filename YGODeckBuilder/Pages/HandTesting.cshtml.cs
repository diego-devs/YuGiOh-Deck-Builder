using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.Models;

namespace YGODeckBuilder.Pages
{

    public class HandTestingModel : PageModel
    {
        private DeckUtility deckUtility { get; set; }
        public Deck deck { get; set; }
        public HandTestingModel(YgoContext db, IConfiguration config)
        {
            deckUtility = new DeckUtility(db, config);
        }

        public void OnGet()
        {
            // Initialize the deck with card data
            InitializeDeck();
        }

        // Add additional methods as needed

        private void InitializeDeck()
        {
            // Generate and return the deck of cards. This is now only using the DeckUtility class
            deck = this.deckUtility.Deck;

            // Generate cards and add them to the deck
            // Add logic to generate cards based on your requirements
        }
    }
}
