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
            LinkData();
            
            // Main Console app for consulting cards. Comment code to test the YgoProDeckTools
            //await MainApplication();


            // Change this path to your local machine path. This should go to a web config ?
            var dataPath = @"C:/Users/PC Gamer/source/repos/YuGiOhTCG/YGOCardSearch/Data";
            var allCardsPath = @"C:/Users/PC Gamer/source/repos/YuGiOhTCG/YGOCardSearch/Data/allCards.json";
            var imagesLocalPath = @"C:/Users/PC Gamer/source/repos/YuGiOhTCG/YGOCardSearch/Data/images";


            // Use this  Main Method to download images and map the correct paths. 

            // Download and save all cards from API
            //var allcards = await YGOProvider.GetAllCardsAsync();

            //YgoProDeckTools.SaveCards(allcards, allCardsPath);


            // Load all cards from json file
            //List<Card> allCards = YgoProDeckTools.ReadAllCards(allCardsPath);

            //Download all images and images small first
            //await YgoProDeckTools.DownloadCardImages(allCards, "small");

            // Map images to correct path in local machine
            //var allCards = YgoProDeckTools.ReadAllCards(allCardsPath);
            //YgoProDeckTools.MapImages(allCards, imagesLocalPath);

            // Save and overwrite modified cards to local folder
            //YgoProDeckTools.SaveCards(allCards, allCardsPath); 
            //Console.WriteLine("All cards and images have been downloaded and mapped to local. ");


            // Add all cards to database
            //await YgoProDeckTools.AddAllCards(allCardsPath);

            
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
        public static void LinkData()
        {
            using (var context = new YgoContext())
            {
                var AllCards = new List<Card>(context.Cards);
                var AllImages = new List<Image>(context.Images);
                var AllSets = new List<CardSet>(context.CardSets);
                var AllPrices = new List<Price>(context.Prices);

                foreach (var Card in AllCards)
                {
                    Card.CardImages = new List<Image>(AllImages.Where(c => c.CardImageId == Card.KonamiCardId)) { };
                    Card.CardSets = new List<CardSet>(AllSets.Where(c => c.CardId == Card.CardId));
                    Card.CardPrices = new List<Price>(AllPrices.Where(c => c.CardId == Card.CardId));

                }
                context.Cards.UpdateRange(AllCards);
                context.SaveChanges();
               
            }
        }
    }
    
}
