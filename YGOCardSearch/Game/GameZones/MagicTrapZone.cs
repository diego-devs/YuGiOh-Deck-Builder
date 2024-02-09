

using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Game.GameZones
{
    public class MagicTrapZone
    {
        public Card[] MagicsTrapsZone { get; set; }

        public void PlaceCard (int pos, Card card)
        {
            if (MagicsTrapsZone[pos] == null)
            {
                MagicsTrapsZone[pos] = card;
            }
            else
            {
                System.Console.WriteLine("magic/trap slot already ocupied");
            }
        }
    }
}