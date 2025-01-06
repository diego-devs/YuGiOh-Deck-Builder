using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.DataProviders
{
    public class YgoAPIProvider : ICardsProvider
    {
        public async Task<ICollection<Card>> GetAllCardsAsync()
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php";
            var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
            try
            {
                var request = await ygoClient.GetAsync(ygoClient.BaseAddress);
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
                    Console.WriteLine("Cards found: " + model.Data.Count);
                    var data = model.Data;

                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new Exception(message: "ërror");
            }

        }
        public async Task<Card> GetCardAsync(int id)
        {
            string url = "http://db.ygoprodeck.com/api/v7/cardinfo.php?id=" + id.ToString();
            var ygoClient= new HttpClient() { BaseAddress = new Uri(url) };
            try
            {
                var request = await ygoClient.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
                    Console.Write("Card found");
                    var data = model.Data[0];

                    return data;
                }
                else 
                {
                    Console.WriteLine("Card not found");
                    return null;
                }
            }
            catch
            {
                throw new Exception(message: "error");
                
            }
        }
        async Task<ICollection<Card>> ICardsProvider.GetSearchAsync(string search)
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?fname=" + search;
            var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };

            try
            {
                var request = await ygoClient.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
                    Console.WriteLine("Cards found: " + model.Data.Count);
                    var data = model.Data;

                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<Card> GetRandomCardAsync() 
        {
            
            string url = "https://db.ygoprodeck.com/api/v7/randomcard.php" ;
            var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
            try
            {
                var request = await ygoClient.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
                    Console.WriteLine("Cards found: " + model.Data.Count);
                    var data = model.Data[0];

                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new Exception(message: "ërror");
            }
        }
        public async Task<List<string>> GetAllCardsIdsAsync() 
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?";

            var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
            var request = await ygoClient.GetAsync(url);
            if (request.IsSuccessStatusCode)
            {
                var content = await request.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
                Console.WriteLine("Cards found: " + model.Data.Count);
                var idList = new List<string>();
                foreach (var card in model.Data)
                {
                    idList.Add(card.CardId.ToString());
                }
                return idList;
            }
            else
            {
                return null;
            }
        }        
    }
}