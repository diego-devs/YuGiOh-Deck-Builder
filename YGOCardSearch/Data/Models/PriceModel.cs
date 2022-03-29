using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Models
{
    public class PriceModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("cardmarket_price")]
        public string CardmarketPrice { get; set; }

        [JsonPropertyName("tcgplayer_price")]
        public string TcgplayerPrice { get; set; }

        [JsonPropertyName("ebay_price")]
        public string EbayPrice { get; set; }

        [JsonPropertyName("amazon_price")]
        public string AmazonPrice { get; set; }

        [JsonPropertyName("coolstuffinc_price")]
        public string CoolstuffincPrice { get; set; }
        
    }
}
