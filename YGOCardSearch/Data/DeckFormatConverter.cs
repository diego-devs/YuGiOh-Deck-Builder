using System.IO;
using System.Linq;
using System.Text.Json;
using YGOCardSearch.Data.Models;
using YGOCardSearch.Data.Models.JsonFormatDeck;

namespace YGOCardSearch.Data
{
    public class DeckFormatConverter
    {
        public static JsonDeck DeserializeJsonDeck(string json)
        {
            return JsonSerializer.Deserialize<JsonDeck>(json);
        }

        public static string SerializeYdkDeck(Deck ydkDeck)
        {
            // Implement your serialization logic here for converting a YdkDeck to JSON
            // For demonstration purposes, let's assume deck is already converted to JSON
            return JsonSerializer.Serialize(ydkDeck);
        }

        public static JsonDeck ConvertYdkToJson(Deck ydkDeck)
        {
            // Implement your conversion logic here from YdkDeck to JsonDeck
            var jsonDeck = new JsonDeck()
            {
                Name = ydkDeck.DeckName,
                Accessory = new JsonDeckAccessory(),
                PickCards = new JsonDeckPickCards(),
                MainDeck = new JsonDeckMain(),
                ExtraDeck = new JsonDeckExtra(),
                SideDeck = new JsonDeckSide()
            };

            // Map main deck cards
            jsonDeck.MainDeck.Ids.AddRange(ydkDeck.MainDeck.Select(card => card.KonamiCardId));
            jsonDeck.MainDeck.Rarities.AddRange(Enumerable.Repeat(1, ydkDeck.MainDeck.Count));

            // Map extra deck cards
            jsonDeck.ExtraDeck.Ids.AddRange(ydkDeck.ExtraDeck.Select(card => card.KonamiCardId));
            jsonDeck.ExtraDeck.Rarities.AddRange(Enumerable.Repeat(1, ydkDeck.ExtraDeck.Count));

            // Map side deck cards
            jsonDeck.SideDeck.Ids.AddRange(ydkDeck.SideDeck.Select(card => card.KonamiCardId));
            jsonDeck.SideDeck.Rarities.AddRange(Enumerable.Repeat(1, ydkDeck.SideDeck.Count));

            // Pick Cards: adding the first 3 cards in the main deck
            jsonDeck.PickCards.Ids.Add(1, jsonDeck.MainDeck.Ids.ElementAt(0));
            jsonDeck.PickCards.Ids.Add(2, jsonDeck.MainDeck.Ids.ElementAt(1));
            jsonDeck.PickCards.Ids.Add(3, jsonDeck.MainDeck.Ids.ElementAt(2));

            // Accessory : Adding a default data


            return jsonDeck;
        }

        public static Deck ConvertJsonToYdk(JsonDeck jsonDeck)
        {
            // Implement your conversion logic here from JsonDeck to YdkDeck
            // For demonstration purposes, let's assume jsonDeck is already converted to YdkDeck
            return new Deck();
        }

        public static string ReadJsonFile(string filePath)
        {
            // Read JSON data from file
            return File.ReadAllText(filePath);
        }

        public static void WriteJsonFile(string filePath, string json)
        {
            // Write JSON data to file
            File.WriteAllText(filePath, json);
        }
    }
}
