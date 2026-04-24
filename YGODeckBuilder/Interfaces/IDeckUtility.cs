using System.Collections.Generic;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Interfaces
{
    public interface IDeckUtility
    {
        Task<Deck> LoadDeckAsync(string path);
        DecksPreviewResult LoadDecksPreview(int? userId = null);
        void PrepareCardData(Deck deck);
        void PrepareCardDataSearch(List<Card> cards);
        void ShuffleDeck();
        List<Card> DrawCards(int count);
        void ExportDeck(Deck deck);
        void ExportDeck(Deck deck, string path);
        string GetUserDecksFolderPath(int userId);
    }
}
