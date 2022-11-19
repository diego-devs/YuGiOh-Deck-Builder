using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    [Table("Images")]
    public class Image
    {
        // Image ID (internal)
        [Key]
        [JsonPropertyName("image_id")]
        public int ImageId { get; set; }

        [JsonPropertyName("id")]
        public int CardImageId { get; set; }

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
