using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGOCardSearch.Models;

namespace YGOCardSearch.Pages
{
    public class CardViewerModel : PageModel
    {
        public ICardsProvider CardsProvider;
        public CardModel Card { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search {get; set;} 
        public string CardId { get; set; }

        public CardViewerModel(ICardsProvider cardsProvider)
        {
            this.CardsProvider = cardsProvider;
        }

        public async Task<ActionResult> OnGet(int id)
        {
            var card = await CardsProvider.GetCardAsync(id);
            if (card != null)
            {
                Card = card;
                return Page();
            }
            return RedirectToPage("Index");
        }
        public static int GenerateRandomIdAsync()
        {
            var CardIdList = JsonSerializer.Deserialize<List<int>>
                (System.IO.File.ReadAllText(@"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\DataLayer\ids.txt"));
            
            Random random = new Random();
            int randomId = CardIdList[random.Next(0, CardIdList.Count)];

            return randomId;
        }

    }
}
