using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YugiohDB.Models;

namespace YugiohDB;
public static class YGOProvider 
{
    /// <summary>
    /// Gets searched card or similar names
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public static async Task<Card> SearchAsync(string search) 
    {
        string url = @"https://db.ygoprodeck.com/api/v7/cardinfo.php?fname=" + search;
            
        var ygoClient = new HttpClient() {BaseAddress = new Uri(url)};
        
        try
        {
            var request = await ygoClient.GetAsync(url);
            if (request != null)
            {
                var content = await request.Content.ReadAsStringAsync();
                var model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
                Console.WriteLine("Cards found: " + model.Data.Count);
                return model;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {

            throw;
        }
        
        
        
    }
    /// <summary>
    /// Downloads to local json file of all cards, response as it is from API ( data object )
    /// </summary>
    /// <returns></returns>
    /// 
    public static async Task DownloadAllCardsAsync(string localPath)
    {
        string apiUrl = "https://db.ygoprodeck.com/api/v7/cardinfo.php";


        using (HttpClient client = new HttpClient())
        using (HttpResponseMessage response = await client.GetAsync(apiUrl))
        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
        using (StreamReader reader = new StreamReader(contentStream))
        {
            string json = await reader.ReadToEndAsync(); // this is text from model 
            var deserialized = JsonConvert.DeserializeObject<Card>(json);
            var cards = deserialized.Data;
            File.WriteAllText(localPath, json);

        }
        
    }

    // Deprecated
    public static async Task<List<Card>> GetAllCardsAsync() 
    {
        string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?"; //Gets all cards!

        var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
        var request = await ygoClient.GetAsync(url);
        if (request.IsSuccessStatusCode)
        {
            var content = await request.Content.ReadAsStringAsync();
            var model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull});
            Console.WriteLine("Cards found: " + model.Data.Count);
            List<Card> allCards = new List<Card>(model.Data);
            return allCards;
                
        }
        else
        {
            Console.WriteLine("Can't connect to ygoprodeck api to download cards");
            return null;
        }
        
    }
    public static async Task<Card> GetCardAsync(string cardId)
    {
        string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?&id=" + cardId; //Gets one card by id

        var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
        var request = await ygoClient.GetAsync(url);
        if (request.IsSuccessStatusCode)
        {
            var content = await request.Content.ReadAsStringAsync();
            Card model = System.Text.Json.JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
            Console.WriteLine("Cards found: " + model.Data.Count);
            Card card = model.Data[0];
            return card ;

        }
        return null;
    }
}