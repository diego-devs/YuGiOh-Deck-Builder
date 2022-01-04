using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YGOCardSearch.Models
{
    
        public partial class ygoModel
        {
            [JsonPropertyName("data")]
            public ygoModel[] Data { get; set; }
        }

        public partial class ygoModel
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("desc")]
            public string Desc { get; set; }

            [JsonPropertyName("atk")]
            public long Atk { get; set; }

            [JsonPropertyName("def")]
            public long Def { get; set; }

            [JsonPropertyName("level")]
            public long Level { get; set; }

            [JsonPropertyName("race")]
            public string Race { get; set; }

            [JsonPropertyName("attribute")]
            public string Attribute { get; set; }

            [JsonPropertyName("card_sets")]
            public CardSet[] CardSets { get; set; }

            [JsonPropertyName("card_images")]
            public CardImage[] CardImages { get; set; }

            [JsonPropertyName("card_prices")]
            public CardPrice[] CardPrices { get; set; }
        }

        public partial class CardImage
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("image_url")]
            public Uri ImageUrl { get; set; }

            [JsonPropertyName("image_url_small")]
            public Uri ImageUrlSmall { get; set; }
        }

        public partial class CardPrice
        {
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

        public partial class CardSet
        {
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
