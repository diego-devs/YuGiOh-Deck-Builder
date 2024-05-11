using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using YGODeckBuilder.DataProviders;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using YGODeckBuilder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using Newtonsoft.Json;
using YGODeckBuilder.Data.Models;
using Microsoft.AspNetCore.Http;
using YGODeckBuilder.API;

namespace YGODeckBuilder.Pages
{
    public class DeckBuilder : PageModel
    {
        // Add IHttpContextAccessor for accessing session
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string decksLocalFolder;
        private DeckUtility deckUtility { get; set; }

        // Deck a visualizar
        public Deck Deck { get; set; }
        // Database
        public readonly YgoContext Context;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DeckFileName { get; set; }
        public List<Card> SearchCards { get; set; }

        // Dependency injection of both Configuration and YgoContext 
        public DeckBuilder(YgoContext db, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            // DI 
            Context = db; 
            _configuration = configuration;
            decksLocalFolder = _configuration["Paths:DecksFolderPath"];
            deckUtility = new DeckUtility(Context, _configuration);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Use DeckPath here to load the specific deck
            if (!string.IsNullOrEmpty(DeckFileName))
            {
                // todo: deck could not exists if renamed 
                // Logic to load deck using DeckPath
                if (System.IO.File.Exists($"{decksLocalFolder}\\{DeckFileName}.ydk"))
                {
                    Deck = await deckUtility.LoadDeckAsync($"{decksLocalFolder}\\{DeckFileName}.ydk");
                    deckUtility.PrepareCardData(Deck);
                    // Store the deck NAME in session storage
                    _httpContextAccessor.HttpContext.Session.SetString("CurrentDeckName", Deck.DeckName);
                } else
                {
                    return RedirectToPage("/Error");
                }
                

                
            }
            else 
            {
                // Retrieve deck from session storage if available
                var currentDeckName = _httpContextAccessor.HttpContext.Session.GetString("CurrentDeckName");
                if (currentDeckName != null)
                {
                    // Logic to load deck using DeckPath
                    Deck = await deckUtility.LoadDeckAsync($"{decksLocalFolder}\\{currentDeckName}.ydk");
                    deckUtility.PrepareCardData(Deck);
                }
                else
                {
                    Deck = new Deck();
                    Deck.DeckName = new string(DateTime.Today.ToShortDateString());
                    Deck.DeckFilePath = $"{decksLocalFolder}\\{Deck.DeckName}.ydk";
                    // dev todo: add binding users deck name
                }
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
                // Return a default result
                var results = Context.GetSearch(_configuration["DefaultSearch"]);
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
