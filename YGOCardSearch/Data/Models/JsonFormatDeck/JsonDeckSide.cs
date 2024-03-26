using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models.JsonFormatDeck
{
    public class JsonDeckSide
    {
        [JsonPropertyName("ids")]
        public List<int> Ids { get; set; }

        [JsonPropertyName("r")]
        public List<int> Rarities { get; set; }
    }
}