using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    public class doughnutChart
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }

    }
}
