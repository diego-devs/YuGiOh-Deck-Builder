using System.Collections.Generic;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Game.GameZones
{
    public class ExtraDeckZone
    {
        public ExtraDeckZone(Deck deck)
        {
            ExtraDeck = new List<Card>(deck.ExtraDeck);
        }
        public List<Card> ExtraDeck { get; set; }
    }
}