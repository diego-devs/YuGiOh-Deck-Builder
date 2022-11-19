using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    [Table("Prices")]
    public class Price
    {
        [Key]
        [JsonPropertyName("price_id")]
        public int PriceId { get; set; }

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
