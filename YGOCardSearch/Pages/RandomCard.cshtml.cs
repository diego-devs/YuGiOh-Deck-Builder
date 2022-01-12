using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGOCardSearch.Models;

namespace YGOCardSearch.Pages
{
    public class RandomCardModel : PageModel
    {
        Random random = new Random();
        public CardModel Card {get; set; }
        public ICardsProvider cardsProvider { get; set; }
        public long CardId {get; set;}

        public RandomCardModel(ICardsProvider cardsProvider)
        {
            this.cardsProvider = cardsProvider;
        }
        
        public async Task<CardModel> GetCard() 
        {
            var randomCard = await cardsProvider.GetRandomCardAsync();
            return randomCard;
        }


        public async Task<IActionResult> OnGet()
        {
            Card = await cardsProvider.GetRandomCardAsync();
            CardId = Card.Id;

            return Page();
        }
    }
}
