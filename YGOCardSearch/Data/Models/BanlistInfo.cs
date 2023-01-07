﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    [Table("BanlistInfo")]
    public class BanlistInfo
    {
        [Key]
        [JsonPropertyName("banlistinfo_id")]
        public int Banlist_Id { get; set; } // Internal care only

        [JsonPropertyName("ban_tcg")]
        public string Ban_TCG { get; set; }
        [JsonPropertyName("ban_ocg")]
        public string Ban_OCG { get; set; }
        [JsonPropertyName("ban_goat")]
        public string Ban_GOAT { get; set; }
    }
}