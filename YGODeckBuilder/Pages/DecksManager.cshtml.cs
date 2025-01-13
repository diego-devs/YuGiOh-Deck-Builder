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
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder.Pages
{
    public class DecksManager : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DeckPreview> Decks { get; set; }
        // Database
        public readonly YgoContext Context;
        private IDeckUtility _deckUtility { get; set; }

        public DecksManager(YgoContext context, IConfiguration configuration, IDeckUtility deckUtility)
        {
            // Load the Database
            this.Context = context;
            _configuration = configuration;
            Decks = new List<DeckPreview>();
            _deckUtility = deckUtility;

        }

        public void OnGet()
        {
            Decks = _deckUtility.LoadDecksPreview();
        }
    }
}
