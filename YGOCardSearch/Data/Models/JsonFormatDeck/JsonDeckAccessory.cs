using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models.JsonFormatDeck
{
    public class JsonDeckAccessory
    {
        [JsonPropertyName("box")]
        public int Box { get; set; }

        [JsonPropertyName("sleeve")]
        public int Sleeve { get; set; }

        [JsonPropertyName("field")]
        public int Field { get; set; }

        [JsonPropertyName("object")]
        public int Object { get; set; }

        [JsonPropertyName("av_base")]
        public int AvBase { get; set; }
    }
}