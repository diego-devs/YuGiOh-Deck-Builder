using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YugiohDB.Models
{
    
        public class ResponseModel
        {
            [JsonPropertyName("data")]
            public ICollection<Card> Data { get; set; }
        }
    
}
