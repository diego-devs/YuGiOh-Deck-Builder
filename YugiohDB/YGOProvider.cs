using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YGODeckBuilder.Data.EntityModels;

namespace YugiohDB;
public static class YGOProvider
{
    private const string BaseUrl = "https://db.ygoprodeck.com/api/v7/";

    public static async Task<Card> SearchAsync(string search)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(BaseUrl + "cardinfo.php?fname=" + Uri.EscapeDataString(search));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
        Console.WriteLine("Cards found: " + model?.Data?.Count);
        return model;
    }

    /// <summary>
    /// Downloads all cards from the API to a local JSON file.
    /// </summary>
    public static async Task DownloadAllCardsAsync(string localPath)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(BaseUrl + "cardinfo.php?misc=yes");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        File.WriteAllText(localPath, json);
    }

    public static async Task<List<Card>> GetAllCardsAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(BaseUrl + "cardinfo.php?misc=yes");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Can't connect to ygoprodeck api to download cards");
            return null;
        }
        var content = await response.Content.ReadAsStringAsync();
        var model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        Console.WriteLine("Cards found: " + model?.Data?.Count);
        return model?.Data != null ? new List<Card>(model.Data) : null;
    }

    public static async Task<Card> GetCardAsync(string cardId)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(BaseUrl + "cardinfo.php?misc=yes&id=" + Uri.EscapeDataString(cardId));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        var model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
        Console.WriteLine("Cards found: " + model?.Data?.Count);
        return model?.Data?[0];
    }

    public static async Task<List<BanlistInfo>> GetAllBanlistAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(BaseUrl + "cardinfo.php?misc=yes&banlist=tcg");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Can't connect to ygoprodeck api to download cards");
            return null;
        }
        var content = await response.Content.ReadAsStringAsync();
        var model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        Console.WriteLine("Cards found: " + model?.Data?.Count);

        var banlist = new List<BanlistInfo>();
        foreach (var card in model.Data)
        {
            banlist.Add(new BanlistInfo()
            {
                CardId = card.KonamiCardId,
                Ban_TCG = card.BanlistInfo.Ban_TCG,
                Ban_OCG = card.BanlistInfo.Ban_OCG,
                Ban_GOAT = card.BanlistInfo.Ban_GOAT
            });
        }
        return banlist;
    }
}
