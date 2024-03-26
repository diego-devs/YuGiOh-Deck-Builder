using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models.JsonFormatDeck
{
    public class JsonDeckPickCards
    {
        [JsonPropertyName("ids")]
        public Dictionary<int, int> Ids { get; set; }

        [JsonPropertyName("r")]
        public Dictionary<int, int> Rarities { get; set; }
    }
}