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
        private const string AppVersion = "v0.1";
        static async Task Main(string[] args)
        {
            // search cards
            await MainApplication();
            

            // This should go to a web config ??
            var path = @"C:/Users/d_dia/source/repos/YuGiOhTCG/YGOCardSearch/Data/allCards.txt";
            //var LocalImagesPath = @"C:/Users/d_dia/source/repos/YuGiOhTCG/YGOCardSearch/Data/images";

            //var allCards = YgoProDeckTools.ReadAllCards(path); // Get all cards from .txt file
            //YgoProDeckTools.MapImages(allCards, LocalImagesPath); // Map images to correct path in local machine
            //YgoProDeckTools.SaveCards(allCards, path); // Save modified cards to local folder
            //Console.WriteLine("Finished.");


            // Currently all cards already added o YgoDB in local SQL instance
            //Add all cards to database
            //await YgoProDeckTools.AddAllCards(path);
        }
        private static async Task MainApplication()
        {
            Console.WriteLine($"YGO DB CARD SEARCH {AppVersion}");
            Console.WriteLine("Welcome");
            START:

            Console.WriteLine("Type your search for a card");
            var userSearch = Console.ReadLine().ToString();
            var search = await YGOProvider.SearchAsync(userSearch);
            if (search != null)
            {
                foreach (var c in search.Data)
                {
                    Console.WriteLine($"Card found:");
                    Console.WriteLine($"Name: {c.Name}");
                    Console.WriteLine($"Type: {c.Type}");
                    if (c.MiscInfo != null)
                    {
                        Console.WriteLine($"tcg Year: {c.MiscInfo.First().TcgDate}");
                    }
                    Console.WriteLine($"Price: {c.CardPrices.First().TcgPlayer.ToString()}");
                }
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Search again? y/n ");
            Console.WriteLine("Quit program: q/Q");
            Console.ResetColor();


            char r = Console.ReadKey().KeyChar;
            if (r == 'Y' || r == 'y')
            {
                goto START;
            }
            else if (r == 'N' || r == 'n')
            {
                QuitProgram();
            }
            else if (r == 'q' || r == 'Q')
            {
                QuitProgram();
            }
            else
            {
                Console.WriteLine("Incorrect answer");
            }
        }
        private static void QuitProgram()
        {
            Console.WriteLine("See ya!");
            Environment.Exit(0);
        }
    }
    
}
