﻿using System.Collections.Generic;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.Game.GameZones;
using YGODeckBuilder.Game.Models;


namespace YGODeckBuilder.Game
{
    public interface IDuelist
    {
        public bool NormalSummon(MonsterCard card);
        public bool NormalSummon(MonsterCard card, List<Card> sacrifices);
        public bool SpecialSummon(MonsterCard card);
        // public bool SpecialSummon(Card card, List<Card> sacrifices);
        public bool SetMonster(MonsterCard card);
        public bool SetMonster(MonsterCard card, List<Card> sacrifices);
        public bool SetSpell(SpellCard card);
        public bool ActivateSpell(SpellCard card);
        public bool SetTrap(TrapCard card);
        public bool ActivateTrap(TrapCard card);
        public bool DeclareAttack(MonsterCard activeMonster, MonsterCard opponentMonster);
        public void AddCardToHand();
        public void AddNCardsToHand();
        public void EndPhase();
        public bool EndTurn();



    }
}