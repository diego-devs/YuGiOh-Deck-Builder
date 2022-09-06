using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YugiohDB.Models;

namespace YugiohDB
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var allCards = YgoProDeckTools.ReadAllCards();
            YgoProDeckTools.MapImages(allCards);

            Console.WriteLine("Finished.");
            Console.WriteLine(allCards[0].CardImages[0].ImageLocalUrl);

            //Save the cards with the correct local image path
            YgoProDeckTools.SaveCards(allCards);
        }

   
    }
    
}
