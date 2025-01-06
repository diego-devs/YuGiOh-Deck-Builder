using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.DataProviders
{
    public class YgoDBProvider : ICardsProvider
    {
        public YgoContext Context {get;set;}

        public YgoDBProvider(YgoContext context) 
        {
            Context = context;
        }

        public async Task<ICollection<Card>> GetSearchAsync(string search)
        {
            string normalizedQuery = search.ToLower().Normalize();

            var matchingCards = await Context.Cards
                .Where(card =>
                    card.Name.ToLower().Contains(normalizedQuery) ||
                    card.Desc.ToLower().Contains(normalizedQuery))
                .ToListAsync();
            if (matchingCards.IsNullOrEmpty())
            {
                Console.WriteLine("Not a single card found.");
            }

            return matchingCards;
        }

        public async Task<ICollection<Card>> GetAllCardsAsync()
        {
            return await Context.Cards.ToListAsync(); 
        }

        public async Task<Card> GetCardAsync(int id)
        {   
            var card = await Context.Cards.SingleAsync(c=>c.KonamiCardId == id);
            if (card != null)
            {
                return card;
            }
            Console.WriteLine($"There is not a card with that ID {id}");
            return null;
            
        }

    }
}