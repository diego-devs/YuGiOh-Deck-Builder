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
    [Table("CardMiscInformation")]
    public class MiscInfo
	{
        [Key]
        [JsonPropertyName("miscinfo_id")]
        public int MiscId { get; set; }
        [JsonPropertyName("card_id")]
        public int CardId { get; set; }
        [JsonPropertyName("beta_name")]
        public string BetaName { get; set; }
        [JsonPropertyName("views")]
        public long Views { get; set; }
        [JsonPropertyName("viewsweek")]
        public int ViewsWeek { get; set; }
        [JsonPropertyName("upvotes")]
        public int UpVotes { get; set; }
        [JsonPropertyName("downvotes")]
        public int DownVotes { get; set; }

        [JsonPropertyName("treated_as")]
        public string TreatedAs { get; set; }
        [JsonPropertyName("tcg_date")]
        public string TcgDate { get; set; }
        [JsonPropertyName("ocg_date")]
        public string OcgDate { get; set; }
        
        [JsonPropertyName("has_effect")]
        public int HasEffect { get; set; }

    }
}
