using System.Collections.Generic;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Game.GameZones
{
    public class GraveyardZone
    {
        public GraveyardZone()
        {
            Graveyard = new List<Card>();
        }

        public static List<Card> Graveyard { get; set; }

        public static void AddToGraveyard(Card card)
        {
            Graveyard.Add(card);
        }
        public static void AddToGraveyard(List<Card> cards)
        {
            Graveyard.AddRange(cards);
        }
    }
}
