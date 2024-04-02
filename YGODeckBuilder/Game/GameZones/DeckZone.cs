using System.Collections.Generic;
using YGODeckBuilder.Data.Models;


namespace YGODeckBuilder.Game.GameZones
{
    public class DeckZone
    {
        public DeckZone(Deck deck)
        {
            Deck = new List<Card>(deck.MainDeck);
        }

        public List<Card> Deck { get; set; }

        public Card DrawCard()
        {
            return null;
        }
    }
}