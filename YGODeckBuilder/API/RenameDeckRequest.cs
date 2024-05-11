using System.Text.Json.Serialization;

namespace YGODeckBuilder.API
{
    public class RenameDeckRequest
    {
        [JsonPropertyName("oldDeckName")]
        public string OldDeckName { get; set; }
        [JsonPropertyName("newDeckName")]
        public string NewDeckName { get; set; }
    }
}
