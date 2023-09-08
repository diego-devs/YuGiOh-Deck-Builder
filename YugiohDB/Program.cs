using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YugiohDB.Models;

namespace YugiohDB
{
    public class Program
    {
        public YgoContext Context;
        private const string AppVersion = "v0.1";
        public static async Task Main(string[] args)
        {
            // change the paths from appsettings.json
            IConfiguration configuration = LoadConfiguration();
            string cardsLocalPath = configuration.GetSection("CardData")["CardsLocalPath"];
            string imagesLocalPath = configuration.GetSection("CardData")["ImagesLocalPath"];
            Console.WriteLine($"Cards Local Path: {cardsLocalPath}");
            Console.WriteLine($"Images Local Path: {imagesLocalPath}");            
            
            // Use this Main method to download images and map the correct paths as you need or to test functionality. 
            // LinkData();

            // 1- Download and save all cards from API
            //var allcards = await YGOProvider.GetAllCardsAsync();
            //YgoProDeckTools.SaveCards(allcards, cardsLocalPath);

            // 2- Load all cards from json file
            //List<Card> localCards = YgoProDeckTools.ReadAllCards(cardsLocalPath);

            // 3- Download all images and images small first
            //await YgoProDeckTools.DownloadImagesAsync(localCards, "cropped");
            //await YgoProDeckTools.DownloadCardImages(allCards, "small");
            //await YgoProDeckTools.DownloadCardImages(allCards, ""

            // 4- Map images to correct path in local machine
            //var allCards = YgoProDeckTools.ReadAllCards(allCardsPath);
            //YgoProDeckTools.MapImages(localCards, imagesLocalPath);

            //YgoProDeckTools.SaveCards(localCards, cardsLocalPath);

            // 5- Map banlist info
            //var banlists = await YGOProvider.GetAllBanlistAsync();
            //YgoProDeckTools.MapBanlistInfo(localCards, banlists);

            // 6- Save and overwrite modified cards to local folder
            //YgoProDeckTools.SaveCards(localCards, cardsLocalPath); 
            //Console.WriteLine("All cards and images have been downloaded and mapped to local. ");

            // 7- Add all cards to database
            //await YgoProDeckTools.AddAllCards(cardsLocalPath);

            await MainApplication(); // Search cards and displays them into console

            static IConfiguration LoadConfiguration()
            {
                return new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
            }

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
        /// <summary>
        /// For mapping correctly the database to a EF object. Database is not all related. 
        /// Maybe this should run only when loading the database and only once. Singleton pattern? 
        /// </summary>
        public static void MapCardData()
        {
            using (var context = new YgoContext())
            {
                var AllCards = new List<Card>(context.Cards);
                var AllImages = new List<CardImages>(context.Images);
                var AllSets = new List<CardSet>(context.CardSets);
                var AllPrices = new List<CardPrices>(context.Prices);

                foreach (var Card in AllCards)
                {
                    Card.CardImages = new List<CardImages>(AllImages.Where(c => c.CardImageId == Card.KonamiCardId)) { };
                    Card.CardSets = new List<CardSet>(AllSets.Where(c => c.CardId == Card.CardId));
                    Card.CardPrices = new List<CardPrices>(AllPrices.Where(c => c.CardId == Card.CardId));

                }
                context.Cards.UpdateRange(AllCards);
                context.SaveChanges();
               
            }
        }
    }
    
}
