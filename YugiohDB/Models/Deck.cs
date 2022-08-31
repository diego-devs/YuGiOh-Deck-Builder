using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YugiohDB.Models
{
    public class Deck
    {
        [Key]
        [JsonPropertyName("DeckId")]
        public int DeckId { get; set; }
        public string DeckName { get; set; }
        public ICollection<Card> MainDeck { get; set; }
        public ICollection<Card> ExtraDeck { get; set; }
        public ICollection<Card> SideDeck { get; set; }
        

        public Deck()
        {
            MainDeck = new List<Card>();
            ExtraDeck = new List<Card>();  
            SideDeck = new List<Card>();   
        }
        
    }
}
