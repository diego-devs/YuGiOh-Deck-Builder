using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YugiohDB.Models;

namespace YugiohDB
{
    public class YgoProDeckTools
    {
        public readonly YgoContext Context;
        /// <summary>
        /// Downloads all card images to local folder. Select the size and location for the images to be downloaded.
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="imgSize">Use "small", "large" or "cropped"</param>
        /// <returns></returns>
        public static async Task DownloadImagesAsync(List<Card> cards, string imgSize)
        {
            if (imgSize == "small")
            {
                string localFolder = "C:/Users/PC Gamer/source/repos/YuGiOhTCG/YGOCardSearch/data/images/small"; // This should be changed to use configuration

                using (HttpClient client = new HttpClient())
                {
                    var count = 0;
                    foreach (Card card in cards)
                    {
                        if (card.CardImages != null)
                        {
                            // Small images
                            foreach (CardImages img in card.CardImages)
                            {
                                var imageUrl = img.ImageUrlSmall;

                                string fileName = $"{card.KonamiCardId}.jpg";
                                string localPath = Path.Combine(localFolder, fileName);

                                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                                using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
                                {
                                    await contentStream.CopyToAsync(fileStream);
                                }

                                //Log
                                Console.WriteLine($"{count++} File {imageUrl} downloaded in {localPath}");
                            }
                            count = 0;


                        }
                    }
                }
                Console.WriteLine($"Completed. {cards.Count} images downloaded to local.");
            }
            else if (imgSize == "large")
            {
                string localFolder = "C:/Users/PC Gamer/source/repos/YuGiOhTCG/YGOCardSearch/data/images"; // This should be changed in prod

                using (HttpClient client = new HttpClient())
                {
                    foreach (Card card in cards)
                    {
                        if (card.CardImages != null)
                        {
                            var count = 0;
                            // Large images
                            foreach (CardImages img in card.CardImages)
                            {
                                var imageUrl = img.ImageUrl;


                                string fileName = $"{card.KonamiCardId}.jpg";
                                string localPath = Path.Combine(localFolder, fileName);

                                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                                using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
                                {
                                    await contentStream.CopyToAsync(fileStream);
                                }

                                //Log
                                Console.WriteLine($"{count++} File {imageUrl} downloaded in {localPath}");
                            }
                            count = 0;


                        }
                    }
                }
                Console.WriteLine($"Completed. {cards.Count} images downloaded to local.");
            }
            else if (imgSize == "cropped")
            {
                string localFolder = "C:/Users/PC Gamer/source/repos/YuGiOhTCG/YGOCardSearch/data/images/cropped"; // This should be changed in prod

                using (HttpClient client = new HttpClient())
                {
                    foreach (Card card in cards)
                    {
                        if (card.CardImages != null)
                        {
                            var count = 0;
                            // Large images
                            foreach (CardImages img in card.CardImages)
                            {
                                var imageUrl = img.ImageUrlCropped;


                                string fileName = $"{card.KonamiCardId}.jpg";
                                string localPath = Path.Combine(localFolder, fileName);

                                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                                using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
                                {
                                    await contentStream.CopyToAsync(fileStream);
                                }

                                //Log
                                Console.WriteLine($"{count++} File {imageUrl} downloaded in {localPath}");
                            }



                        }
                    }
                }
                Console.WriteLine($"Completed. {cards.Count} images downloaded to local.");
            }
            else
            {
                Console.WriteLine("Wrong img size parameter. Use small, large or cropped");
            }
        }
        
        /// <summary>
        /// Serializes and writes a list of cards to the selected path
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="path"></param>
        public static void SaveCards(List<Card> cards, string path)
        {
            var serializedCards = JsonSerializer.Serialize(cards);
            File.WriteAllText(path, serializedCards);
            Console.WriteLine($"{cards.Count} cards saved to: " + path);
        }
        /// <summary>
        /// Loads all cards from json file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Card> ReadAllCards(string path)
        {
            var r = File.ReadAllText(path);
            // Option for serialization, hoping to avoid null values
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
        /// <summary>
        /// Maps the images path to the local path
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="LocalImagesPath"></param>
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
                        img.ImageLocalUrlCropped = LocalImagesPath + $"/cropped/{img.CardImageId}.jpg";
                    }
                }


            }
            Console.WriteLine($"All cards linked to local images at {LocalImagesPath}");
        }
        public static void MapBanlistInfo(List<Card> cards, List<BanlistInfo> banlist)
        {
            foreach (var bl in banlist)
            {
                var card = cards.FirstOrDefault(c => c.KonamiCardId == bl.CardId);
                card.BanlistInfo = bl;

            }


        }
    }
}
