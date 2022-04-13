
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YugiohDB.Models
{
    public class Card
    {
        [JsonIgnore]
        [JsonPropertyName("data")]
        public ICollection<Card> Data { get; set; }

        [Key]
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
        public ICollection<Object> CardSets { get; set; }

        [JsonPropertyName("card_images")]
        public ICollection<Object> CardImages { get; set; }

        [JsonPropertyName("card_prices")]
        public ICollection<Object> CardPrices { get; set; }
    }

    

    

   

}
