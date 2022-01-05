using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YGOCardSearch.Models;

namespace YGOCardSearch.DataProviders
{
    public class YGOProvider : ICardsProvider
    {
        private readonly List<CardModel> repo = new List<CardModel>();
        public async Task<ICollection<CardModel>> GetAllCardsAsync()
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php";
            var ygoClient = new HttpClient() {BaseAddress = new Uri(url)};
            try
            {
                var request = await ygoClient.GetAsync(ygoClient.BaseAddress);
                if (request.IsSuccessStatusCode) 
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<CardModel>(content, new JsonSerializerOptions());
                    Console.WriteLine("Cards found: " + model.Data.Length);
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
                throw new Exception(message:"ërror");
            }
            
        }

        

        public Task<CardModel> GetCardAsync(int id)
        {
            return null;
            //string url = "http://db.ygoprodeck.com/api/v7/cardinfo.php?id=" + id.ToString();
        }

        async Task<ICollection<CardModel>> ICardsProvider.GetSearchAsync(string search)
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?fname=" + search;
            var ygoClient = new HttpClient() {BaseAddress = new Uri(url)};
            
            try 
            {
                var request = await ygoClient.GetAsync(url);
                if (request.IsSuccessStatusCode) 
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<CardModel>(content, new JsonSerializerOptions());
                    Console.WriteLine("Cards found: " + model.Data.Length);
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
                throw new Exception(message:"ërror");
            }
            
        }
    }
}