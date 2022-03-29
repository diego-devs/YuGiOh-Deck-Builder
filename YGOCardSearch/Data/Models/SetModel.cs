using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Models
{
    public partial class SetModel
    { 
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("set_name")]
        public string SetName { get; set; }

        [JsonPropertyName("set_code")]
        public string SetCode { get; set; }

        [JsonPropertyName("set_rarity")]
        public string SetRarity { get; set; }

        [JsonPropertyName("set_rarity_code")]
        public string SetRarityCode { get; set; }

        [JsonPropertyName("set_price")]
        public string SetPrice { get; set; }
    }
}
