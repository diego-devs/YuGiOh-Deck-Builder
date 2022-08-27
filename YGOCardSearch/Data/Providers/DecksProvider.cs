using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using YGOCardSearch.Interfaces;
using YGOCardSearch.Models;

namespace YGOCardSearch.DataProviders
{
    public class DecksProvider : IDecksProvider
    {
        public Task<DeckModel> GetDeck(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
