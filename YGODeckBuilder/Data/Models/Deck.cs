using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YGODeckBuilder.Data.Models
{
    // This is the primary Deck class we use to Load and Export .YDK deck files 
    // Deck class for all decks in application
    
    [Table("Decks")]
    public class Deck
    {
        [Key]
        [JsonPropertyName("deck_id")]
        public int DeckId { get; set; }
        [JsonPropertyName("deck_name")]
        public string DeckName { get; set; }

        [JsonPropertyName("main_deck")]
        public ICollection<Card> MainDeck { get; set; } = [];
        [JsonPropertyName("extra_deck")]
        public ICollection<Card> ExtraDeck { get; set; } = [];
        [JsonPropertyName("side_deck")]
        public ICollection<Card> SideDeck { get; set; } = [];
        [JsonPropertyName("total_cards")]
        public int TotalCards { get { return totalCards; } 
                                set { totalCards = MainDeck.Count + ExtraDeck.Count; } }
        private int totalCards;

        [JsonPropertyName("deck_file_path")]
        public string DeckFilePath { get; set; }


        public Deck(string deckName, ICollection<Card> mainDeck, ICollection<Card> extraDeck, ICollection<Card> sideDeck, string deckFilePath)
        {
            DeckName = deckName;
            MainDeck = mainDeck;
            ExtraDeck = extraDeck;
            SideDeck = sideDeck;
            DeckFilePath = deckFilePath;
        }

        public Deck(string deckName)
        {
            DeckName = deckName;
            MainDeck = new List<Card>();
            ExtraDeck = new List<Card>();
            SideDeck = new List<Card>();
        }

        public Deck()
        {

        }


    }
}
