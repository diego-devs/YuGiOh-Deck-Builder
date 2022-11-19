using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
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
