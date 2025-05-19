using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGODeckBuilder.Data.EntityModels
{
    [Table("Collections")]
    public class Collection
    {
        public Collection()
        {
            CreationDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("worth")]
        public double Worth { get; set; }
        [JsonPropertyName("creation_date")]
        public DateTime CreationDate { get; set; }
        [JsonPropertyName("last_modified_date")]
        public DateTime LastModifiedDate { get; set; }
        [JsonPropertyName("collection_cards")]
        public List<CollectionCard> CollectionCards { get; set; } = new();
    }
}
