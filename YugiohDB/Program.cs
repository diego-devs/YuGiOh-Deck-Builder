using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YugiohDB
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var connectionString = configuration.GetConnectionString("YGODatabase");
            var cardsLocalPath = configuration.GetValue<string>("Paths:CardIdsFilePath");
            var imagesLocalPath = configuration.GetValue<string>("Paths:ImagesFolder");

            if (string.IsNullOrWhiteSpace(connectionString) ||
                string.IsNullOrWhiteSpace(cardsLocalPath) ||
                string.IsNullOrWhiteSpace(imagesLocalPath))
            {
                Console.WriteLine("Missing required configuration (connection string / Paths). Check appsettings.json.");
                return;
            }

            Console.WriteLine($"Cards JSON path: {cardsLocalPath}");
            Console.WriteLine($"Images folder: {imagesLocalPath}");

            var dbOptions = new DbContextOptionsBuilder<YgoContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var httpClient = new HttpClient();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("==== YugiohDB Tool ====");
                Console.WriteLine("[1] Download all cards from API → local JSON");
                Console.WriteLine("[2] Download all card images (small, large, cropped)");
                Console.WriteLine("[3] Map local image paths + banlist into cards JSON");
                Console.WriteLine("[4] Push cards JSON into database");
                Console.WriteLine("[5] Interactive card search");
                Console.WriteLine("[Q] Quit");
                Console.Write("> ");

                var choice = Console.ReadLine()?.Trim().ToLowerInvariant();
                switch (choice)
                {
                    case "1":
                        await DownloadAndSaveCards(cardsLocalPath);
                        break;
                    case "2":
                        await DownloadAllImages(cardsLocalPath, imagesLocalPath, httpClient);
                        break;
                    case "3":
                        await MapImagesAndBanlist(cardsLocalPath, imagesLocalPath);
                        break;
                    case "4":
                        await YgoProDeckTools.AddAllCards(cardsLocalPath, dbOptions);
                        break;
                    case "5":
                        await SearchApplication();
                        break;
                    case "q":
                        Console.WriteLine("See ya!");
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }

        private static async Task DownloadAndSaveCards(string cardsLocalPath)
        {
            var allCards = await YGOProvider.GetAllCardsAsync();
            if (allCards == null)
            {
                Console.WriteLine("Failed to fetch cards from API.");
                return;
            }
            YgoProDeckTools.SaveCardsFile(allCards, cardsLocalPath);
        }

        private static async Task DownloadAllImages(string cardsLocalPath, string imagesLocalPath, HttpClient client)
        {
            if (!File.Exists(cardsLocalPath))
            {
                Console.WriteLine($"Cards JSON not found at {cardsLocalPath}. Run [1] first.");
                return;
            }
            var localCards = YgoProDeckTools.ReadAllCards(cardsLocalPath);
            await YgoProDeckTools.DownloadImagesAsync(localCards, CardImageSize.Large, imagesLocalPath, client);
            await YgoProDeckTools.DownloadImagesAsync(localCards, CardImageSize.Small, imagesLocalPath, client);
            await YgoProDeckTools.DownloadImagesAsync(localCards, CardImageSize.Cropped, imagesLocalPath, client);
        }

        private static async Task MapImagesAndBanlist(string cardsLocalPath, string imagesLocalPath)
        {
            if (!File.Exists(cardsLocalPath))
            {
                Console.WriteLine($"Cards JSON not found at {cardsLocalPath}. Run [1] first.");
                return;
            }
            var localCards = YgoProDeckTools.ReadAllCards(cardsLocalPath);
            YgoProDeckTools.MapImages(localCards, imagesLocalPath);

            var banlists = await YGOProvider.GetAllBanlistAsync();
            if (banlists != null)
                YgoProDeckTools.MapBanlistInfo(localCards, banlists);

            YgoProDeckTools.SaveCardsFile(localCards, cardsLocalPath);
            Console.WriteLine("Cards JSON updated with image paths + banlist.");
        }

        private static async Task SearchApplication()
        {
            Console.WriteLine("---- Card Search ----");
            while (true)
            {
                Console.Write("Search (empty to quit): ");
                var userSearch = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userSearch)) return;

                var search = await YGOProvider.SearchAsync(userSearch);
                if (search?.Data == null || !search.Data.Any())
                {
                    Console.WriteLine("No results.");
                    continue;
                }

                foreach (var c in search.Data)
                {
                    Console.WriteLine($"Name: {c.Name}");
                    Console.WriteLine($"Type: {c.Type}");
                    var tcgDate = c.MiscInfo?.FirstOrDefault()?.TcgDate;
                    if (tcgDate != null) Console.WriteLine($"TCG Year: {tcgDate}");
                    var price = c.CardPrices?.FirstOrDefault()?.TcgPlayer;
                    Console.WriteLine($"Price: {price ?? "N/A"}");
                    Console.WriteLine();
                }
            }
        }
    }
}
