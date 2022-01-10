using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using YGOCardSearch.Models;

namespace YGOCardSearch.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; } = "dragon";
        public ICardsProvider cardsProvider { get; set; }
        public List<CardModel> Cards { get; set; }
        public CardModel Card { get; set; }

        public IndexModel(ICardsProvider cardsProvider)
        {
            this.cardsProvider = cardsProvider;
        }
        public static double GetCurrentChange(string price) 
        {
            double returnPrice = Convert.ToDouble(price);
            return returnPrice * 20.55;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrWhiteSpace(Search))
            {
                var results = await cardsProvider.GetSearchAsync(Search);
                if (results != null)
                {
                    Cards = new List<CardModel>(results);
                }
            }
            else
            {
                var results = await cardsProvider.GetAllCardsAsync();
                if (results != null)
                {
                    Cards = new List<CardModel>(results);
                };

            }
            return Page();
        }
    }
}
