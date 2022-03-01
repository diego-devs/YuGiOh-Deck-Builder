using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YGOCardSearch.Models;

namespace YGOCardSearch
{
    public interface ICardsProvider
    {
        public Task<ICollection<CardModel>> GetSearchAsync(string search);
        public Task<ICollection<CardModel>> GetAllCardsAsync();
        public Task<CardModel> GetCardAsync(int id);
        public Task<CardModel> GetRandomCardAsync();

    }
}