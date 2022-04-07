using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YugiohDB.Models
{
    public class CardPrice
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
