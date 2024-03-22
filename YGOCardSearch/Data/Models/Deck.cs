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

        [JsonPropertyName("main_deck")]
        public ICollection<Card> MainDeck { get; set; }
        [JsonPropertyName("extra_deck")]
        public ICollection<Card> ExtraDeck { get; set; }
        [JsonPropertyName("side_deck")]
        public ICollection<Card> SideDeck { get; set; }
        [JsonPropertyName("total_cards")]
        public int TotalCards { get { return totalCards; } 
                                set { totalCards = MainDeck.Count + ExtraDeck.Count; } }
        private int totalCards;

        [JsonPropertyName("deck_file_path")]
        public string DeckFilePath { get; set; }


    }
}
