using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch
{
    public interface ICardsProvider
    {
        public Task<ICollection<Card>> GetSearchAsync(string search);
        public Task<ICollection<Card>> GetAllCardsAsync();
        public Task<Card> GetCardAsync(int id);
        public Task<Card> GetRandomCardAsync();

    }
}