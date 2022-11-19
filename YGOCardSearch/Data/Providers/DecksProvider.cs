using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using YGOCardSearch.Interfaces;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.DataProviders
{
    public class DecksProvider : IDecksProvider
    {
        public Task<Deck> GetDeck(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
