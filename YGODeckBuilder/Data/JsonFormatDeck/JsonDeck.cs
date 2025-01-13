using System.Text.Json.Serialization;

namespace YGODeckBuilder.Data.JsonFormatDeck
{
    public class JsonDeck
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ct")]
        public long CreationTimestamp { get; set; }

        [JsonPropertyName("et")]
        public long LastEditTimestamp { get; set; }

        [JsonPropertyName("accessory")]
        public JsonDeckAccessory Accessory { get; set; }

        [JsonPropertyName("pick_cards")]
        public JsonDeckPickCards PickCards { get; set; }

        [JsonPropertyName("m")]
        public JsonDeckMain MainDeck { get; set; }

        [JsonPropertyName("e")]
        public JsonDeckExtra ExtraDeck { get; set; }

        [JsonPropertyName("s")]
        public JsonDeckSide SideDeck { get; set; }
    }
}
