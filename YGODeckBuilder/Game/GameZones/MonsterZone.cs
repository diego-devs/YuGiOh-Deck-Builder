using System.Net.Http.Headers;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Game.GameZones
{
    public class MonsterZone
    {
        public Card[] MonstersZone { get; set; }

        public void AddCard(int pos, Card card)
        {
            if (MonstersZone[pos] == null)
            {
                this.MonstersZone[pos] = card;
            }
            else 
            {
                System.Console.WriteLine("monster slot ocupied");
            }
        }
        public void SendToGraveyard(Card card)
        {

        }
    }
}