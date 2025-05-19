using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace YGODeckBuilder.Data.EntityModels
{
    [Table("CollectionCards")]
    public class CollectionCard
    {
        [Key]
        [JsonPropertyName("collection_card_id")]
        public int CollectionCardId { get; set; }

        [ForeignKey("Collection")]
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        [JsonPropertyName("collection")]
        public Collection Collection { get; set; }

        [ForeignKey("Card")]
        [JsonPropertyName("card_id")]
        public int CardId { get; set; }

        [JsonPropertyName("card")]
        public Card Card { get; set; }

        // Represents the number of copies of the card in the collection
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        // Optional: To record when a card was added to the collection
        [JsonPropertyName("date_added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
