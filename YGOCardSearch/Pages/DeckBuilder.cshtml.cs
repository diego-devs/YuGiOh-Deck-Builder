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
        private readonly string decksLocalFolder;

        // Deck a visualizar
        public Deck Deck { get; set; }
        // Database
        public readonly YgoContext Context;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DeckFileName { get; set; }

        public List<Card> SearchCards { get; set; }
        private DeckUtility deckUtility { get; set; }
        

        // Dependency injection of both Configuration and YgoContext 
        public DeckBuilder(YgoContext db, IConfiguration configuration)
        {
            // Good place to initialize data
            this.Context = db; 
            this._configuration = configuration;
            decksLocalFolder = _configuration["Paths:DecksFolderPath"];
            this.deckUtility = new DeckUtility(Context, _configuration);

           
        }
        public async Task<IActionResult> OnGetAsync()
        {
            // Use DeckPath here to load the specific deck
            if (!string.IsNullOrEmpty(DeckFileName))
            {
                // Logic to load deck using DeckPath
                Deck = await deckUtility.LoadDeck($"{decksLocalFolder}\\{DeckFileName}.ydk");
                deckUtility.PrepareCardData(Deck);
            }

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var results = Context.GetSearch(SearchQuery);
                if (results != null)
                {
                    // Prepare card infos
                    deckUtility.PrepareCardDataSearch(results);

                    SearchCards = new List<Card>(results);
                    
                }
            }
            else
            {
                var results = Context.GetSearch("dark ruler");
                if (results != null)
                {
                    // Prepare card infos
                    deckUtility.PrepareCardDataSearch(results);

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
