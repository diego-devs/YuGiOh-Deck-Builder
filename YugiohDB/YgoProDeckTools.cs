using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YugiohDB
{
    public enum CardImageSize
    {
        Small,
        Large,
        Cropped
    }

    public class YgoProDeckTools
    {
        public static async Task DownloadImagesAsync(List<Card> cards, CardImageSize size, string imagesRoot, HttpClient client)
        {
            string subfolder = size switch
            {
                CardImageSize.Small => "small",
                CardImageSize.Large => "",
                CardImageSize.Cropped => "cropped",
                _ => throw new ArgumentOutOfRangeException(nameof(size))
            };
            string localFolder = Path.Combine(imagesRoot, subfolder);
            Directory.CreateDirectory(localFolder);

            int total = cards.Sum(c => c.CardImages?.Count ?? 0);
            int downloaded = 0;
            int failed = 0;

            foreach (var card in cards)
            {
                if (card.CardImages == null) continue;

                foreach (var img in card.CardImages)
                {
                    string url = size switch
                    {
                        CardImageSize.Small => img.ImageUrlSmall,
                        CardImageSize.Large => img.ImageUrl,
                        CardImageSize.Cropped => img.ImageUrlCropped,
                        _ => null
                    };
                    if (string.IsNullOrEmpty(url)) continue;

                    string localPath = Path.Combine(localFolder, $"{card.KonamiCardId}.jpg");
                    try
                    {
                        using var response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        using var contentStream = await response.Content.ReadAsStreamAsync();
                        using var fileStream = new FileStream(localPath, FileMode.Create);
                        await contentStream.CopyToAsync(fileStream);
                        downloaded++;
                        if (downloaded % 50 == 0)
                            Console.WriteLine($"[{downloaded}/{total}] {size} images downloaded");
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        Console.WriteLine($"Failed {url}: {ex.Message}");
                    }
                }
            }
            Console.WriteLine($"Completed {size}: {downloaded}/{total} downloaded, {failed} failed → {localFolder}");
        }

        public static void SaveCardsFile(List<Card> cards, string path)
        {
            var serializedCards = JsonSerializer.Serialize(cards);
            File.WriteAllText(path, serializedCards);
            Console.WriteLine($"{cards.Count} cards saved to: {path}");
        }

        public static List<Card> ReadAllCards(string path)
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Deserialize<List<Card>>(json, options);
        }

        public static async Task AddAllCards(string allCardsPath, DbContextOptions<YgoContext> dbOptions)
        {
            var allCards = ReadAllCards(allCardsPath);

            using var context = new YgoContext(dbOptions);
            try
            {
                context.Cards.AddRange(allCards);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Changes have not been made");
                Console.WriteLine(e.Message);
                if (e.InnerException != null)
                    Console.WriteLine(e.InnerException.Message);
            }
            finally
            {
                Console.WriteLine($"from total {allCards.Count} cards");
                Console.WriteLine($"{context.Cards.Count()} cards saved in {context.Database.ProviderName}");
            }
        }

        public static void MapImages(List<Card> cards, string localImagesPath)
        {
            foreach (var card in cards)
            {
                if (card.CardImages == null) continue;
                foreach (var img in card.CardImages)
                {
                    img.ImageLocalUrl = $"{localImagesPath}/{img.CardImageId}.jpg";
                    img.ImageLocalUrlSmall = $"{localImagesPath}/small/{img.CardImageId}.jpg";
                    img.ImageLocalUrlCropped = $"{localImagesPath}/cropped/{img.CardImageId}.jpg";
                }
            }
            Console.WriteLine($"All cards linked to local images at {localImagesPath}");
        }

        public static void MapBanlistInfo(List<Card> cards, List<BanlistInfo> banlist)
        {
            foreach (var bl in banlist)
            {
                var card = cards.FirstOrDefault(c => c.KonamiCardId == bl.CardId);
                if (card != null)
                    card.BanlistInfo = bl;
            }
        }
    }
}
