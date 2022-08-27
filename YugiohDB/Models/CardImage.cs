using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YugiohDB.Models
{
    public class CardImage
    {
        [Key]
        [JsonPropertyName("InternalId")]
        public int InternalId { get; set; }

        [JsonPropertyName("id")]
        public int CardId { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("image_url_small")]
        public string ImageUrlSmall { get; set; }
    }
}
