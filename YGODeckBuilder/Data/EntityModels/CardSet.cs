using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGODeckBuilder.Data.EntityModels
{
    [Table("CardSets")]
    public class CardSet
    {
        [Key]
        [JsonPropertyName("cardset_id")]
        public int CardSetId { get; set; }
        [JsonPropertyName("card_id")]
        public int CardId { get; set; }

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
