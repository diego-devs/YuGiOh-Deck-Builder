using System.Collections.Generic;
using YGOCardSearch.Data.Models;
using YGOCardSearch.Game.GameZones;
using YGOCardSearch.Game.Models;

namespace YGOCardSearch.Game
{
    public class Player : IDuelist
    {
        public static Deck Deck { get; set; }
        
        public static int LifePoints { get; set; }
        public GraveyardZone GraveyardZone { get; set; }
        public HandZone HandZone { get; set; }
        public MonsterZone MonsterZone { get; set; }
        public MagicTrapZone MagicTrapZone { get; set; }
        public DeckZone DeckZone { get; set; }
        public ExtraDeckZone ExtraDeckZone { get; set; }

        public void Draw()
        {
            var c = DeckZone.DrawCard();
            HandZone.AddCard(c);
        }
        public static void PrepareGame()
        {
            
            LifePoints = 8000;
            
        }

        public bool NormalSummon(MonsterCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool NormalSummon(MonsterCard card, List<Card> sacrifices)
        {
            throw new System.NotImplementedException();
        }

        public bool SpecialSummon(MonsterCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool SetMonster(MonsterCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool SetMonster(MonsterCard card, List<Card> sacrifices)
        {
            throw new System.NotImplementedException();
        }

        public bool SetSpell(SpellCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool ActivateSpell(SpellCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool SetTrap(TrapCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool ActivateTrap(TrapCard card)
        {
            throw new System.NotImplementedException();
        }

        public bool DeclareAttack(MonsterCard activeMonster, MonsterCard opponentMonster)
        {
            throw new System.NotImplementedException();
        }

        public void AddCardToHand()
        {
            throw new System.NotImplementedException();
        }

        public void AddNCardsToHand()
        {
            throw new System.NotImplementedException();
        }

        public void EndPhase()
        {
            throw new System.NotImplementedException();
        }

        public bool EndTurn()
        {
            throw new System.NotImplementedException();
        }
    }
}
