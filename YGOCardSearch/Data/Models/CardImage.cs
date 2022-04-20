using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    [Keyless]
    public class CardImage
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("image_url_small")]
        public string ImageUrlSmall { get; set; }

    }
}
