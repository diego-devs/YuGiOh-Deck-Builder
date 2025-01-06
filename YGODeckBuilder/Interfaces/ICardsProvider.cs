using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder
{
    public interface ICardsProvider
    {
        public Task<ICollection<Card>> GetSearchAsync(string search); // developer todo: add options
        public Task<ICollection<Card>> GetAllCardsAsync();
        public Task<Card> GetCardAsync(int id);

    }
}