using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGOCardSearch.Data.Models
{
    [Table("Decks")]
    public class Deck
    {
        [Key]
        [JsonPropertyName("deck_id")]
        public int DeckId { get; set; }
        [JsonPropertyName("deck_name")]
        public string DeckName { get; set; }
        public ICollection<Card> MainDeck { get; set; }
        public ICollection<Card> ExtraDeck { get; set; }
        public ICollection<Card> SideDeck { get; set; }



    }
}
