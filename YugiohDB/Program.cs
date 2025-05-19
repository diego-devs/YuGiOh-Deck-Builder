using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YugiohDB
{
    public class Program
    {
        public YGODeckBuilder.Data.YgoContext Context;
        public static async Task Main(string[] args)
        {
            string option = DisplayWelcomeScreen();

            switch (option)
            {
                case "1":
                    await MainApplication();
                    break;
                case "2":
                    var _configuration = SetupConfiguration();
                    string configOption = DisplayConfigurationOptions();
                    switch (configOption)
                    {
                        case "1":
                            Console.WriteLine("Option 1: Download and save all cards");
                            var allcards = await YGOProvider.GetAllCardsAsync();
                            ApiDatabaseHelper.SaveCardsFile(allcards, _configuration.CardsLocalPath);
                            Console.WriteLine("Cards downloaded and saved in local path successfully.");
                            break;

                        case "2":
                            Console.WriteLine("Option 2: Download and map card images");
                            var localCards = ApiDatabaseHelper.ReadAllCards(_configuration.CardsLocalPath);
                            await ApiDatabaseHelper.DownloadImagesAsync(localCards, CardImageSize.Big);
                            await ApiDatabaseHelper.DownloadImagesAsync(localCards, CardImageSize.Small);
                            await ApiDatabaseHelper.DownloadImagesAsync(localCards, CardImageSize.Cropped);
                            ApiDatabaseHelper.MapImages(localCards, _configuration.ImagesLocalPath);
                            Console.WriteLine("Images downloaded and mapped in local path successfully.");
                            break;

                        case "3":
                            Console.WriteLine("Option 3: Map banlist information");
                            var cards = ApiDatabaseHelper.ReadAllCards(_configuration.CardsLocalPath);
                            var banlists = await YGOProvider.GetAllBanlistAsync();
                            ApiDatabaseHelper.MapBanlistInfo(cards, banlists);
                            ApiDatabaseHelper.SaveCardsFile(cards, _configuration.CardsLocalPath);
                            Console.WriteLine("Banlist information mapped successfully.");
                            break;

                        case "4":
                            Console.WriteLine("Option 4: Map card data");
                            ApiDatabaseHelper.MapCardData();
                            Console.WriteLine("Card data mapped successfully.");
                            break;

                        case "5":
                            Console.WriteLine("Option 5: Save local cards to database");
                            var cardsToSave = ApiDatabaseHelper.ReadAllCards(_configuration.CardsLocalPath);
                            await ApiDatabaseHelper.AddCardsToDatabaseAsync(_configuration.CardsLocalPath);
                            using (var context = new YgoContext())
                            {
                                await context.Database.EnsureCreatedAsync();
                                await context.Cards.AddRangeAsync(cardsToSave);
                                await context.SaveChangesAsync();
                            }
                            Console.WriteLine("Cards saved to database successfully.");
                            break;

                        default:
                            Console.WriteLine("Invalid option selected.");
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option selected. Exiting...");
                    QuitProgram();
                    break;
            }
        }
        private static async Task RunDevelopmentTasks(ConfigurationStructure _configuration)
        {
            // Legacy development tasks> 
            // 1- Download and save all cards from API
            //var allcards = await YGOProvider.GetAllCardsAsync();
            //YgoProDeckTools.SaveCardsFile(allcards, cardsLocalPath);

            //// 2- Load all cards from json file
            //List<Card> localCards = YgoProDeckTools.ReadAllCards(cardsLocalPath);

            //// 3- Download all images and images sizes *** developer todo: some cards have more than 1 image
            //await YgoProDeckTools.DownloadImagesAsync(localCards, CardImageSize.Big);
            //await YgoProDeckTools.DownloadImagesAsync(localCards, CardImageSize.Small);
            //await YgoProDeckTools.DownloadImagesAsync(localCards, CardImageSize.Cropped);

            //// 4- Map images to correct path in local machine
            //var allCards = YgoProDeckTools.ReadAllCards(cardsLocalPath);
            //YgoProDeckTools.MapImages(localCards, imagesLocalPath);

            //YgoProDeckTools.SaveCardsFile(localCards, cardsLocalPath);
            //// 5- Map banlist info
            //var banlists = await YGOProvider.GetAllBanlistAsync();
            //YgoProDeckTools.MapBanlistInfo(localCards, banlists);
             //// 6- Save and overwrite modified cards to local folder
            //YgoProDeckTools.SaveCardsFile(localCards, cardsLocalPath); 
            //Console.WriteLine("All cards and images have been downloaded and mapped to text file in local path. ");
            var allcards = await YGOProvider.GetAllCardsAsync();
            ApiDatabaseHelper.SaveCardsFile(allcards, _configuration.CardsLocalPath);
            
            List<Card> localCards = ApiDatabaseHelper.ReadAllCards(_configuration.CardsLocalPath);

            await ApiDatabaseHelper.DownloadImagesAsync(localCards, CardImageSize.Big);
            await ApiDatabaseHelper.DownloadImagesAsync(localCards, CardImageSize.Small);
            await ApiDatabaseHelper.DownloadImagesAsync(localCards, CardImageSize.Cropped);

            ApiDatabaseHelper.MapCardData();
        }

        private static ConfigurationStructure SetupConfiguration()
        {
            // Access configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("YGODatabase");
            var decksFolderPath = config.GetValue<string>("Paths:DecksFolderPath");
            var cardsLocalPath = config.GetValue<string>("Paths:CardIdsFilePath");
            var imagesLocalPath = config.GetValue<string>("Paths:ImagesFolder");

            var configuration = new ConfigurationStructure(connectionString, decksFolderPath, cardsLocalPath, imagesLocalPath);
            LogConfiguration(connectionString, decksFolderPath, cardsLocalPath, imagesLocalPath);

            return configuration;

            static void LogConfiguration(string connectionString, string decksFolderPath, string cardsLocalPath, string imagesLocalPath)
            {
                Console.WriteLine($"Connection String: {connectionString}");
                Console.WriteLine($"Decks Folder Path: {decksFolderPath}");
                Console.WriteLine($"Cards Local Path: {cardsLocalPath}");
                Console.WriteLine($"Images Local Path: {imagesLocalPath}");
            }
        }
        private static async Task MainApplication()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;

            Console.WriteLine($"YGO DB CARD SEARCH");
            Console.WriteLine("Welcome");
            Console.WriteLine("This tool will help you locate data for any card or element allocated in your local YuGiOh Database");

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
        public static void UpdateCardDatabase() 
        {
            // get all cards from ygoprodeck api provider
            // compare our database context against those cards retrieved from API
            // identify the new cards and isolate them
            // get the new images of these new cards
            // map the new images correctly in local
            // map the banlist of new cards 
            // add new cards to context
            // get all cards from context and compare again against the ygoprodeck file
        }
        private static string DisplayWelcomeScreen()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    ██╗   ██╗██╗   ██╗ ██████╗ ██╗ ██████╗ ██╗  ██╗
    ╚██╗ ██╔╝██║   ██║██╔════╝ ██║██╔═══██╗██║  ██║
     ╚████╔╝ ██║   ██║██║  ███╗██║██║   ██║███████║
      ╚██╔╝  ██║   ██║██║   ██║██║██║   ██║██╔══██║
       ██║   ╚██████╔╝╚██████╔╝██║╚██████╔╝██║  ██║
       ╚═╝    ╚═════╝  ╚═════╝ ╚═╝ ╚═════╝ ╚═╝  ╚═╝
    ██████╗ ███████╗ ██████╗ ██╗  ██╗    ██████╗ ╗██╗   ██╗██╗██╗     ██████╗ ███████╗██████╗ 
    ██╔══██╗██╔════╝██╔════╝ ██║ ██╔╝    ██╔══██╗║██║   ██║██║██║     ██╔══██╗██╔════╝██╔══██╗
    ██║  ██║█████╗  ██║     ╗█████╔╝     ██████╔╝║██║   ██║██║██║     ██   ██╔█████╗  ███████╔╝
    ██║  ██║██╔══╝  ██║     ║██╔═██╗     ██╔══██╗║██║   ██║██║██║     ██╔══██╗██╔══╝  ██╔═██╗
    ██████╔╝███████╗╚██████╔╝██║  ██╗    ██████╔╝║╚██████╔╝██║███████╗██████╔╝███████╗██║  ███║
    ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═╝    ╚═════╝ ╚═╝ ╚═════╝ ╚═╝╚══════╝╚═════╝ ╚══════╝╚═╝  ╚═╝");
            Console.ResetColor();
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. Search Cards");
            Console.WriteLine("2. Download/Update Card Database");
            Console.ResetColor();

            var option = Console.ReadLine();
            return option;
        }
        private static string DisplayConfigurationOptions()
        {
            Console.WriteLine("\nYGO Database Configuration Options:");
            Console.WriteLine("1. Download latest cards from YGOProDeck API to local path");
            Console.WriteLine("2. Download and map card images to local path");
            Console.WriteLine("3. Map banlist information");
            Console.WriteLine("4. Map card data");
            Console.WriteLine("5. Save local cards to database");
            //Console.WriteLine("5. Run all development tasks (legacy)");
            Console.Write("\nSelect an option (1-4): ");

            var configOption = Console.ReadLine();
            return configOption;
        }
        public class ConfigurationStructure
        {
            public string ConnectionString { get; set; }
            public string DecksFolderPath { get; set; }
            public string CardsLocalPath { get; set; }
            public string ImagesLocalPath { get; set; }

            public ConfigurationStructure(string connectionString, string decksFolderPath, string cardsLocalPath, string imagesLocalPath)
            {
                ConnectionString = connectionString;
                DecksFolderPath = decksFolderPath;
                CardsLocalPath = cardsLocalPath;
                ImagesLocalPath = imagesLocalPath;
            }
        }
    }
    
}
