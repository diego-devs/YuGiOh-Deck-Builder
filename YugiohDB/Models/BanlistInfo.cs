using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace YugiohDB.Models
{
    public class BanlistInfo
    {
        [JsonPropertyName("banlistinfo_id")]
        public long BanlistInfoID { get; set; } // Internal care only
        [JsonPropertyName("ban_tcg")]
        public string Ban_TCG { get; set; }
        [JsonPropertyName("ban_ocg")]
        public string Ban_OCG { get; set; }
        [JsonPropertyName("ban_goat")]
        public string Ban_GOAT { get; set; }
    }




}
