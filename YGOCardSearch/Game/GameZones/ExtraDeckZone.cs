using System.Collections.Generic;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Game.GameZones
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