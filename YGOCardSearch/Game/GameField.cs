using System.Reflection.Metadata;
using YGOCardSearch.Game.GameZones;

namespace YGOCardSearch.Game
{
    public class GameField
    {
        public GraveyardZone GraveyardZone { get; set; }
        public HandZone HandZone { get; set; }
        public MonsterZone MonsterZone { get; set; }
        public MagicTrapZone MagicTrapZone { get; set; }
        public DeckZone DeckZone { get; set; }
        public ExtraDeckZone ExtraDeckZone { get; set; }
    }
}
