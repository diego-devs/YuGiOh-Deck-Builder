using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGODeckBuilder.Data.Models
{
    [Table("BanlistInfo")]
    public class BanlistInfo
    {
        [Key]
        [JsonPropertyName("banlistinfo_id")]
        public int BanlistId { get; set; } // Internal care only

        [JsonPropertyName("card_id")]
        public int CardId { get; set; }

        [JsonPropertyName("ban_tcg")]
        public string Ban_TCG { get; set; }
        [JsonPropertyName("ban_ocg")]
        public string Ban_OCG { get; set; }
        [JsonPropertyName("ban_goat")]
        public string Ban_GOAT { get; set; }
    }
}
