using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YugiohDB.Models
{
    public class CardImage
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
