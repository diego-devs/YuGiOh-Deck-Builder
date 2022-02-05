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
        
        public static int GenerateRandomIdAsync() 
        {
            var CardIdList = JsonSerializer.Deserialize<List<int>>
                (System.IO.File.ReadAllText(@"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\data\ids.txt"));

            Random random = new Random();
            int randomId = CardIdList[random.Next(0, CardIdList.Count)];

            return randomId ;
        }
    }
}
