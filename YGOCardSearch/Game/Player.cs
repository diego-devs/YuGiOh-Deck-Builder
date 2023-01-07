using YGOCardSearch.Data.Models;
using YGOCardSearch.Game.GameZones;

namespace YGOCardSearch.Game
{
    public class Player
    {
        public static Deck Deck { get; set; }
        public GameField GameField { get; set; }
        public static int LifePoints { get; set; }
        public GraveyardZone GraveyardZone { get; set; }
        public HandZone HandZone { get; set; }
        public MonsterZone MonsterZone { get; set; }
        public MagicTrapZone MagicTrapZone { get; set; }
        public DeckZone DeckZone { get; set; }
        public ExtraDeckZone ExtraDeckZone { get; set; }

        public static void Draw()
        {
            var c = DeckZone.DrawCard();
            HandZone.AddCard(c);
        }
        public static void PrepareGame()
        {
            DeckZone = new DeckZone(Deck);
            LifePoints = 8000;
            
        }

    }
}
