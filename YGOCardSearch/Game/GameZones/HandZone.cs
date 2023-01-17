using System.Collections.Generic;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Game.GameZones
{
    public class HandZone
    {
        public static List<Card> Hand { get; set; }
        public static void DrawHand(int n_ofCards, List<Card> deck)
        {

        }
        public static void AddCard(Card card)
        {
            Hand.Add(card);
        }
    }
}