using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace YugiohDB
{
    public static class YugiohCards 
    {
        public static List<CardModel> Cards {get; set;}
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            
            
            // Where cards are saved
            var path = @"C:\Users\d_dia\source\repos\YuGiOhTCG\YugiohDB\data\allCards.txt";
            System.IO.File.Create(path);

            // Create all cards in a text file to /YGOCardSearch/data
            var allCards = await Connection.GetAllCardsAsync();
            YugiohCards.Cards = new List<CardModel>(allCards);


            var jsonCards = JsonSerializer.Serialize<List<CardModel>>(YugiohCards.Cards, new JsonSerializerOptions());
            
            System.IO.File.WriteAllText(path, jsonCards);


            Thread.Sleep(3000);
            Environment.Exit(0); 

        Start:

            Console.WriteLine("Write your search and press enter:");
            var search = Console.ReadLine();

            var CardList = await Connection.SearchAsync(search);
            foreach (var card in CardList.Data) 
            {
                Console.WriteLine($"{card.Name} / {card.Type} ");
            }

            Console.WriteLine("New search? y/n:");
            var response = Console.ReadLine();
            if (response == "y" || response == "Y") {goto Start;}
            else if (response == "n" || response == "N") {Environment.Exit(0);}     
        }

    }
    public static class Connection 
    {
        public static async Task<CardModel> SearchAsync(string search) 
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?fname=" + search;
            
            var ygoClient = new HttpClient() {BaseAddress = new Uri(url)};
            var request = await ygoClient.GetAsync(url);
            if (request.IsSuccessStatusCode) 
            {
                var content = await request.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<CardModel>(content, new JsonSerializerOptions());
                Console.WriteLine("Cards found: " + model.Data.Length);
                return model;
            }
            else 
            {
                return null; 
            }
        }
        public static async Task<IEnumerable<CardModel>> GetAllCardsAsync() 
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?"; //Gets all cards!

            var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
            var request = await ygoClient.GetAsync(url);
            if (request.IsSuccessStatusCode)
            {
                var content = await request.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<CardModel>(content, new JsonSerializerOptions());
                Console.WriteLine("Cards found: " + model.Data.Length);
                var allCards = model.Data;
                return allCards;
                
            }
            return null;
        }

        public static async void SaveAllCards() 
        {
            var allCards = await GetAllCardsAsync();
            
        }
    }
}
