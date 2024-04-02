using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGODeckBuilder.Data.Models.JsonFormatDeck;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.Models;

namespace YGODeckBuilder.Pages
{
    public class DeckFormatConverterModel : PageModel
    {
        public bool ConversionCompleted { get; set; } = false;
        public void OnGet()
        {
            // Example usage
            string json = DeckFormatConverter.ReadJsonFile("deck.json");
            JsonDeck jsonDeck = DeckFormatConverter.DeserializeJsonDeck(json);

            Deck ydkDeck = new Deck(); // Assuming ydkDeck is initialized somehow
            string serializedYdkDeck = DeckFormatConverter.SerializeYdkDeck(ydkDeck);

            JsonDeck convertedJsonDeck = DeckFormatConverter.ConvertYdkToJson(ydkDeck);
            Deck convertedYdkDeck = DeckFormatConverter.ConvertJsonToYdk(jsonDeck);

            DeckFormatConverter.WriteJsonFile("converted_deck.json", DeckFormatConverter.SerializeYdkDeck(convertedYdkDeck));
            ConversionCompleted = true;
        }
    }
}
