using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.DataProviders
{
    public class YgoAPIProvider : ICardsProvider
    {
        private const string BaseUrl = "https://db.ygoprodeck.com/api/v7/";
        private readonly IHttpClientFactory _httpClientFactory;

        public YgoAPIProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("ygoprodeck");

        public async Task<ICollection<Card>> GetAllCardsAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync(BaseUrl + "cardinfo.php");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
            return model?.Data;
        }

        public async Task<Card> GetCardAsync(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync(BaseUrl + "cardinfo.php?id=" + id);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
            return model?.Data?[0];
        }

        public async Task<ICollection<Card>> GetSearchAsync(string search)
        {
            var client = CreateClient();
            var response = await client.GetAsync(BaseUrl + "cardinfo.php?fname=" + Uri.EscapeDataString(search));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
            return model?.Data;
        }

        public async Task<Card> GetRandomCardAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync(BaseUrl + "randomcard.php");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            // randomcard.php returns a single Card object directly, not wrapped in { data: [] }
            return JsonSerializer.Deserialize<Card>(content, new JsonSerializerOptions());
        }

        public async Task<List<string>> GetAllCardsIdsAsync()
        {
            var cards = await GetAllCardsAsync();
            if (cards == null) return null;
            var ids = new List<string>();
            foreach (var card in cards)
                ids.Add(card.CardId.ToString());
            return ids;
        }
    }
}
