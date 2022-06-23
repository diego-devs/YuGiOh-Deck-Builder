using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    public class PriceModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("cardmarket_price")]
        public string CardMarket { get; set; }
        [JsonPropertyName("tcgplayer_price")]
        public string TcgPlayer { get; set; }
        [JsonPropertyName("ebay_price")]
        public string Ebay { get; set; }
        [JsonPropertyName("amazon_price")]
        public string Amazon { get; set; }
        [JsonPropertyName("coolstuffinc_price")]
        public string CoolStuffInc { get; set; }
    }
}
