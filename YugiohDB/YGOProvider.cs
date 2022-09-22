using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
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
                var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
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
    /// Gets searched card by TCG id
    /// </summary>
    /// <returns></returns>
    public static async Task<IEnumerable<Card>> GetAllCardsAsync() 
    {
        string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?"; //Gets all cards!

        var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
        var request = await ygoClient.GetAsync(url);
        if (request.IsSuccessStatusCode)
        {
            var content = await request.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
            Console.WriteLine("Cards found: " + model.Data.Count);
            var allCards = model.Data;
            return allCards;
                
        }
        return null;
    }
    public static async Task<Card> GetCardAsync(string cardId)
    {
        string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?&id=" + cardId; //Gets one card by id

        var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
        var request = await ygoClient.GetAsync(url);
        if (request.IsSuccessStatusCode)
        {
            var content = await request.Content.ReadAsStringAsync();
            Card model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
            Console.WriteLine("Cards found: " + model.Data.Count);
            Card card = model.Data[0];
            return card ;

        }
        return null;
    }
}