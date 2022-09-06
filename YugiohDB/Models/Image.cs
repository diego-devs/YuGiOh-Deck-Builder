using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YugiohDB.Models
{
    public class Image
    {
        // Image ID (internal)
        [Key]
        [JsonPropertyName("image_id")]
        public long ImageId { get; set; }

        [JsonPropertyName("id")]
        public long CardImageId { get; set; }
        
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("image_url_small")]
        public string ImageUrlSmall { get; set; }

        [JsonPropertyName("ImageLocalUrl")]
        public string ImageLocalUrl { get; set; }
        [JsonPropertyName("ImageLocalUrlSmall")] 
        public string ImageLocalUrlSmall { get; set; }
    }
}
