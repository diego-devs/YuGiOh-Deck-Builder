using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YGOCardSearch.Data;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Pages
{
    public class DecksManager : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DeckView> Decks { get; set; }
        // Database
        public readonly YgoContext Context;

        private DeckUtility _deckUtility { get; set; }

        public DecksManager(YgoContext context, IConfiguration configuration)
        {
            // Load the Database
            this.Context = context;
            _configuration = configuration;
            Decks = new List<DeckView>();

        }

        public void OnGet()
        {
            var decks = _deckUtility.LoadDecksView();
        }

       





    }
}
