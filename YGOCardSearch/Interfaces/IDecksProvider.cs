using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YGOCardSearch.Models;

namespace YGOCardSearch.Interfaces
{
    public interface IDecksProvider
    {
        public Task<DeckModel> GetSearchAsync(string path);
        
    }
}

