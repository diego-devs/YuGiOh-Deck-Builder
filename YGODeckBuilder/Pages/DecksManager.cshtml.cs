using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using YGODeckBuilder.API;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.Models;

namespace YGODeckBuilder.Pages
{
    public class DecksManager : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DeckPreview> Decks { get; set; }
        // Database
        public readonly YgoContext Context;

        private DeckUtility _deckUtility { get; set; }

        public DecksManager(YgoContext context, IConfiguration configuration)
        {
            // Load the Database
            this.Context = context;
            _configuration = configuration;
            Decks = new List<DeckPreview>();
            _deckUtility = new DeckUtility(Context, _configuration);

        }

        public void OnGet()
        {
            Decks = _deckUtility.LoadDecksPreview();
        }

        //public async Task<IActionResult> DuplicateDeckAsync(string selectedDeckName)
        //{
        //    int counter = 0;
        //    if (Path.Exists(_configuration["Paths:DeckFolderPath"])) 
        //    {
        //        counter++;
        //    }
        //    var newDeckName = selectedDeckName + $"{counter++}";

        //    Deck selectedDeck = await _deckUtility.LoadDeckAsync($"{_configuration["Paths:DecksFolderPath"]}\\{newDeckName}.ydk");
        //    var res = _deckUtility.ExportDeck(selectedDeck);
            
        //    if (res)
        //    {
        //        return Page();
        //    } 
        //    else
        //    {
        //        var message = "Deck duplicate unsuccessful result";
        //        Console.WriteLine(message);
        //        return BadRequest(new { message });
        //    }

    

    }
}
