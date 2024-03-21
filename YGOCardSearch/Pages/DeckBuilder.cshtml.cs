using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
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
using YGOCardSearch.Data.Models;

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

        private DeckUtility deckUtility { get; set; }
        

        // Dependency injection of both Configuration and YgoContext 
        public DeckBuilder(YgoContext db, IConfiguration configuration)
        {
            // Good place to initialize data
            // Load the Database
            this.Context = db; 
            this._configuration = configuration;
            this.decksPath = _configuration["Paths:DecksFolderPath"];
            this.deckUtility = new DeckUtility(Context, _configuration);


            LoadedDecks = new List<Deck>();
            Deck = new Deck();
            // Load all decks as a list of decks
            LoadedDecks.Add(deckUtility.LoadDeck(decksPath));

            // Get the selected Deck as focus deck
            Deck = LoadedDecks.FirstOrDefault(); // developer todo: make selection with dropdrown menu 
            currentDeckName = Deck.DeckName;

            // Prepare card images, sets and prices from all sub decks:
            deckUtility.PrepareCardData(Deck);
            
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

    }
}
