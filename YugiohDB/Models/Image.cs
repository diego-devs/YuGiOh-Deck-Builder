using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YugiohDB.Models
{
    public class Image
    {
        // Image ID (internal)
        [Key]
        [JsonPropertyName("InternalImageId")]
        public int ImageId { get; set; }

        [JsonPropertyName("id")]
        public int CardId { get; set; }
        
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("image_url_small")]
        public string ImageUrlSmall { get; set; }

        [JsonPropertyName("ImageLocalUrl")]
        public string ImageLocalUrl { get; set; }
        [JsonPropertyName("ImageLocalUrlSmal")] 
        public string ImageLocalUrlSmal { get; set; }
    }
}
