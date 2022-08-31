using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    public class SetModel
    {
        [Key]

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("set_name")]
        public string Name { get; set; }
        [JsonPropertyName("set_code")]
        public string Code { get; set; }
        [JsonPropertyName("set_rarity")]
        public string Rarity { get; set; }
        [JsonPropertyName("set_rarity_code")]
        public string RarityCode { get; set; }
        [JsonPropertyName("set_price")]
        public string Price { get; set; }
    }
}
