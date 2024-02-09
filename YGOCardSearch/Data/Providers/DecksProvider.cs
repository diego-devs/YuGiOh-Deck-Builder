using System.Threading.Tasks;
using YGOCardSearch.Data.Models;
using YGOCardSearch.Interfaces;

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
