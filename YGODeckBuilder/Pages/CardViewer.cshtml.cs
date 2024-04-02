using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using YGODeckBuilder.Data.Models;

namespace YGODeckBuilder.Pages
{
    public class CardViewerModel : PageModel
    {
        public readonly ICardsProvider _cardsProvider;
        public readonly IConfiguration _configuration;
        public Card Card { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search {get; set;} 
        public string CardId { get; set; }

        public CardViewerModel(ICardsProvider cardsProvider, IConfiguration configuration)
        {
            this._cardsProvider = cardsProvider;
            _configuration = configuration;
        }

        public async Task<ActionResult> OnGet(int id)
        {
            var card = await _cardsProvider.GetCardAsync(id);
            if (card != null)
            {
                Card = card;
                return Page();
            }
            return RedirectToPage("Index");
        }
        

    }
}

