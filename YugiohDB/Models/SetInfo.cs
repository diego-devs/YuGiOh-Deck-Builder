using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YugiohDB.Models
{
    public class SetInfo
    {
        [Key]
        [JsonPropertyName("setinfo_id")]
        public long SetInfoId { get; set; }
        [JsonPropertyName("set_name")]
        public string SetName { get; set; }
        [JsonPropertyName("set_code")]
        public string SetCode { get; set; }
        [JsonPropertyName("num_of_cards")]
        public int NumOfCards { get; set; }
        [JsonPropertyName("tcg_date")]
        public string TcgDate { get; set; }
    }
}
