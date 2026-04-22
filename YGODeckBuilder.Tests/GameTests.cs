using System.Collections.Generic;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.Game;
using YGODeckBuilder.Game.GameZones;

namespace YGODeckBuilder.Tests
{
    public class GameTests
    {
        [Fact]
        public void Player_PrepareGame_SetsLifePointsTo8000()
        {
            Player.LifePoints = 0;
            Player.PrepareGame();
            Assert.Equal(8000, Player.LifePoints);
        }

        [Fact]
        public void Turn_Constructor_AssignsActivePlayer()
        {
            var player = new Player();
            var turn = new Turn(player);
            Assert.Same(player, turn.ActivePlayer);
        }

        [Fact]
        public void GameManager_PrepareGame_DoesNotThrow()
        {
            GameManager.PrepareGame();
            var gm = new GameManager { turnNumber = 1 };
            Assert.Equal(1, gm.turnNumber);
        }

        [Fact]
        public void DeckZone_Constructor_CopiesMainDeckCards()
        {
            var deck = new Deck("t")
            {
                MainDeck = new List<Card>
                {
                    new Card { KonamiCardId = 1 },
                    new Card { KonamiCardId = 2 }
                }
            };

            var zone = new DeckZone(deck);

            Assert.Equal(2, zone.Deck.Count);
            // The zone's list must be a separate reference (defensive copy).
            Assert.NotSame(deck.MainDeck, zone.Deck);
        }

        [Fact]
        public void DeckZone_DrawCard_ReturnsNullInCurrentImplementation()
        {
            var zone = new DeckZone(new Deck("t") { MainDeck = new List<Card> { new Card() } });
            Assert.Null(zone.DrawCard());
        }

        [Fact]
        public void ExtraDeckZone_Constructor_CopiesExtraDeckCards()
        {
            var deck = new Deck("t")
            {
                ExtraDeck = new List<Card> { new Card { KonamiCardId = 99 } }
            };

            var zone = new ExtraDeckZone(deck);

            Assert.Single(zone.ExtraDeck);
            Assert.Equal(99, zone.ExtraDeck[0].KonamiCardId);
        }

        [Fact]
        public void HandZone_AddCard_AppendsToStaticHand()
        {
            HandZone.Hand = new List<Card>();
            var card = new Card { KonamiCardId = 7 };

            HandZone.AddCard(card);

            Assert.Single(HandZone.Hand);
            Assert.Equal(7, HandZone.Hand[0].KonamiCardId);
        }

        [Fact]
        public void GraveyardZone_AddCard_AppendsSingleCard()
        {
            GraveyardZone.Graveyard = new List<Card>();
            GraveyardZone.AddToGraveyard(new Card { KonamiCardId = 3 });
            Assert.Single(GraveyardZone.Graveyard);
        }

        [Fact]
        public void GraveyardZone_AddRange_AppendsAllCards()
        {
            GraveyardZone.Graveyard = new List<Card>();
            GraveyardZone.AddToGraveyard(new List<Card>
            {
                new Card { KonamiCardId = 1 },
                new Card { KonamiCardId = 2 },
                new Card { KonamiCardId = 3 }
            });
            Assert.Equal(3, GraveyardZone.Graveyard.Count);
        }

        [Fact]
        public void MonsterZone_AddCard_FillsEmptySlot()
        {
            var zone = new MonsterZone { MonstersZone = new Card[5] };
            var card = new Card { KonamiCardId = 11 };

            zone.AddCard(0, card);

            Assert.Same(card, zone.MonstersZone[0]);
        }

        [Fact]
        public void MonsterZone_AddCard_DoesNotOverrideOccupiedSlot()
        {
            var first = new Card { KonamiCardId = 1 };
            var second = new Card { KonamiCardId = 2 };
            var zone = new MonsterZone { MonstersZone = new Card[5] };

            zone.AddCard(0, first);
            zone.AddCard(0, second);

            Assert.Same(first, zone.MonstersZone[0]);
        }

        [Fact]
        public void MagicTrapZone_PlaceCard_FillsEmptySlot()
        {
            var zone = new MagicTrapZone { MagicsTrapsZone = new Card[5] };
            var card = new Card { KonamiCardId = 22 };

            zone.PlaceCard(0, card);

            Assert.Same(card, zone.MagicsTrapsZone[0]);
        }

        [Fact]
        public void MagicTrapZone_PlaceCard_DoesNotOverrideOccupiedSlot()
        {
            var first = new Card { KonamiCardId = 1 };
            var second = new Card { KonamiCardId = 2 };
            var zone = new MagicTrapZone { MagicsTrapsZone = new Card[5] };

            zone.PlaceCard(0, first);
            zone.PlaceCard(0, second);

            Assert.Same(first, zone.MagicsTrapsZone[0]);
        }
    }
}
