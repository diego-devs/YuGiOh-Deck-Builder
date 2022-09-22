using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YugiohDB.Models;

namespace YugiohDB
{
    public class YgoProDeckTools
    {
        
        public static void DownloadToLocal()
        {
            string path = "C:/Users/d_dia/source/repos/YuGiOhTCG/YugiohDB/data/images/"; // Change this path to your local directory // should be on a web config file
            var allCards = YgoProDeckTools.ReadAllCards(path);
            YgoProDeckTools.DownloadAllImages(allCards);

            Console.WriteLine("All cards succesfully downloaded at: " + path);
        }
        public static void DownloadAllImages(List<Card> AllCards)
        {
            string path = "C:/Users/d_dia/source/repos/YuGiOhTCG/YugiohDB/data/images/"; // This should be changed in prod
            WebClient client = new WebClient();
            foreach (Card card in AllCards)
            {
                if (card.CardImages != null)
                {
                    foreach (Image img in card.CardImages)
                    {
                        var url = img.ImageUrl;
                        
                        client.DownloadFile(new Uri(url), $"{path}" + $"{card.CardId}.jpg");

                        //Log
                        Console.WriteLine($"File {url} downloaded in {path} {card.CardId}.jpg");
                    }
                }
            }
            Console.WriteLine($"Completed. {AllCards.Count} images downloaded to local.");
            Console.WriteLine($"Elapsed time: ");
        }
        public static void DownloadAllImagesSmall(List<Card> AllCards)
        {
            string path = "C:/Users/d_dia/source/repos/YuGiOhTCG/YugiohDB/data/images/small/"; // This should be changed in prod. Perhaps could be fix with web config
            WebClient client = new WebClient();
            foreach (Card card in AllCards)
            {
                if (card.CardImages != null)
                {
                    foreach (Image img in card.CardImages)
                    {
                        var url = img.ImageUrlSmall;

                        client.DownloadFile(new Uri(url), $"{path}" + $"{card.CardId}.jpg");

                        //Log
                        Console.WriteLine($"File {url} downloaded in {path} {card.CardId}.jpg");
                    }
                }
            }
            Console.WriteLine($"Completed. {AllCards.Count} images downloaded to local.");
            Console.WriteLine($"Elapsed time: ");
        }
        public static async Task DownloadImage(Card card)
        {
            string path = "C:/Users/d_dia/source/repos/YuGiOhTCG/YugiohDB/data/images/"; // This should be changed in prod
            
            var url = card.CardImages[0].ImageUrl;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), $"{path}" + $"{card.CardId}.jpg");
            }
        }
        public static void SaveCards(List<Card> cards, string path)
        {
            var serializedCards = JsonSerializer.Serialize(cards);
            File.WriteAllText(path, serializedCards);
            Console.WriteLine($"{cards.Count} cards saved to: " + path);
        }
        public static List<Card> ReadAllCards(string path)
        {
            var r = File.ReadAllText(path);
            // Option for serialization, hoping to avoid null values
            JsonSerializerOptions options = new()
            {
            //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            // all cards from json file deserialized to a list
            List<Card> readedCards = JsonSerializer.Deserialize<List<Card>>(r, options);
            return readedCards;
        }
        /// <summary>
        /// Adds all cards from json file to Database
        /// </summary>
        /// <returns></returns>
        public static async Task AddAllCards(string allCardsPath)
        {
            // This is the process to add all cards to Database
            // path to allcards.txt file json file

            var allCards = ReadAllCards(allCardsPath);
            // Option for serialization, hoping to avoid null values

            // Connect to database in Artemis
            using (var context = new YgoContext())
            //using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Add all the cards to database excecuting SQL commands and using EF
                    //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [YgoDB].[dbo].[Cards] ON");
                    context.Cards.AddRange(allCards);
                    context.SaveChanges();
                    //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [YgoDB].[dbo].[Cards] OFF");
                    //transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Changes have not been made");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException.Message);
                }
                finally
                {
                    Console.WriteLine($"from total {allCards.Count} cards");
                    Console.WriteLine($"{context.Cards.Count()} cards saved in {context.Database.ProviderName}");
                }

            }
        }
        public static void MapImages(List<Card> cards, string LocalImagesPath)
        {
            foreach (var card in cards)
            {
                if (card.CardImages != null)
                {
                    foreach (var img in card.CardImages)
                    {
                        img.ImageLocalUrl = LocalImagesPath + $"/{img.CardImageId}.jpg";
                        img.ImageLocalUrlSmall = LocalImagesPath + $"/small/{img.CardImageId}.jpg";
                    }
                }
               
            }
            Console.WriteLine($"All cards linked to local images at {LocalImagesPath}");
        }
    }
}
