using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Models
{
    public class ImageModel
    {
       
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonPropertyName("image_url_small")]
        public Uri ImageUrlSmall { get; set; }
       

    }
}
