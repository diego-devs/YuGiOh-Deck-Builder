using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YugiohDB
{
    class Program
    {
        static async Task Main(string[] args)
        {
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
                Console.WriteLine("found: " + model.Data.Length);
                return model;
            }
            else 
            {
                return null; 
            }
        }
    }
}
