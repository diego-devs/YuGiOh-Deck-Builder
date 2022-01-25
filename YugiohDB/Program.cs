using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace YugiohDB
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            // var IdsList = await Connection.GetAllCardsIds();

            // var serializedList = JsonSerializer.Serialize(IdsList, new JsonSerializerOptions()) ;
            
            // File.WriteAllText(@"C:\Users\d_dia\source\repos\YuGiOhTCG\YugiohDB\data\ids.txt", serializedList);
            // Console.WriteLine("\nTodas las cartas ids han sido guardadas exitosamente");


            // Console.WriteLine("IDS Created and saved");
            // Thread.Sleep(3000);
            // Environment.Exit(0); 

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
        public static async Task<ygoModel> SearchAsync(string search) 
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?fname=" + search;
            
            var ygoClient = new HttpClient() {BaseAddress = new Uri(url)};
            var request = await ygoClient.GetAsync(url);
            if (request.IsSuccessStatusCode) 
            {
                var content = await request.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<ygoModel>(content, new JsonSerializerOptions());
                Console.WriteLine("Cards found: " + model.Data.Length);
                return model;
            }
            else 
            {
                return null; 
            }
        }
        public static async Task<List<long>> GetAllCardsIds() 
        {
            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php?";

            var ygoClient = new HttpClient() { BaseAddress = new Uri(url) };
            var request = await ygoClient.GetAsync(url);
            if (request.IsSuccessStatusCode)
            {
                var content = await request.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<ygoModel>(content, new JsonSerializerOptions());
                Console.WriteLine("Cards found: " + model.Data.Length);
                List<long> CardsIds = new List<long>();

                foreach (var card in model.Data) 
                {
                    CardsIds.Add(card.Id);
                }
                return CardsIds;
            }
            else
            {
                return null;
            }
        }
    }
}
