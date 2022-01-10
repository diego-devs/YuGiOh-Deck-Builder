using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGOCardSearch.Models;

namespace YGOCardSearch.Pages
{
    public class CardsViewerModel : PageModel
    {
        public ICardsProvider cardsProvider { get; set; }
        public List<CardModel> Cards { get; set; }
        public CardModel Card { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search {get; set;} 
        public string CardId { get; set; }

        public CardsViewerModel(ICardsProvider cardsProvider)
        {
            this.cardsProvider = cardsProvider;
        }

        public async Task<ActionResult> OnGet(int id)
        {
            var card = await cardsProvider.GetCardAsync(id);
            if (card != null)
            {
                Card = card;
                return Page();
            }
            return RedirectToPage("Index");
        }

    }
}
