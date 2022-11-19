using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.Interfaces
{
    public interface IDecksProvider
    {
        // Load all decks
        // Load one deck from file
        public Task<Deck> GetDeck(string path);
        
    }
}

