using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YugiohDB.Models
{
    [Table("SetInformation")]
    public class SetInfo
    {
        [Key]
        [JsonPropertyName("setinfo_id")]
        public int SetId { get; set; }
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
