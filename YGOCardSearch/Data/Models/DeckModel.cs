using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    public class DeckModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public string DeckName { get; set; }
        public ICollection<CardModel> MainDeck { get; set; }
        public ICollection<CardModel> ExtraDeck { get; set; }
        public ICollection<CardModel> SideDeck { get; set; }


        public DeckModel()
        {
            MainDeck = new List<CardModel>();
            ExtraDeck = new List<CardModel>();
            SideDeck = new List<CardModel>();
        }
    }
}
