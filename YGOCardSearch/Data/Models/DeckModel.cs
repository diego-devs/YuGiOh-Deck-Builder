using System.Collections.Generic;

namespace YGOCardSearch.Models
{
    public class DeckModel
    {
        public int Id { get; set; }
        public List<CardModel> MainDeck { get; set; }
        public List<CardModel> ExtraDeck { get; set; }
        public List<CardModel> SideDeck { get; set; }
        public string DeckName { get; set; }

        public DeckModel()
        {
            MainDeck = new List<CardModel>();
            ExtraDeck = new List<CardModel>();  
            SideDeck = new List<CardModel>();   
        }
    }
}
