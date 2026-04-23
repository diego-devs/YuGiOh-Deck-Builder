using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.Data.JsonFormatDeck;

namespace YGODeckBuilder.Tests
{
    public class DeckFormatConverterTests : IDisposable
    {
        private readonly string _tempFile;

        public DeckFormatConverterTests()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), "ygo_conv_" + Guid.NewGuid() + ".json");
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile)) File.Delete(_tempFile);
        }

        [Fact]
        public void DeserializeJsonDeck_ReturnsNullForLiteralJsonNull()
        {
            var result = DeckFormatConverter.DeserializeJsonDeck("null");
            Assert.Null(result);
        }

        [Fact]
        public void DeserializeJsonDeck_ParsesNameField()
        {
            var json = "{\"name\":\"Blue-Eyes\"}";
            var result = DeckFormatConverter.DeserializeJsonDeck(json);
            Assert.NotNull(result);
            Assert.Equal("Blue-Eyes", result.Name);
        }

        [Fact]
        public void SerializeYdkDeck_RoundTripsDeckName()
        {
            var deck = new Deck("Warriors")
            {
                MainDeck = new List<Card>(),
                ExtraDeck = new List<Card>(),
                SideDeck = new List<Card>()
            };

            var json = DeckFormatConverter.SerializeYdkDeck(deck);

            Assert.Contains("Warriors", json);
            using var doc = JsonDocument.Parse(json);
            Assert.True(doc.RootElement.TryGetProperty("deck_name", out _));
        }

        [Fact]
        public void ConvertYdkToJson_CurrentlyThrowsBecauseJsonDeckSubobjectsHaveNullLists()
        {
            // Documents a latent bug: JsonDeckMain/Extra/Side/PickCards are constructed
            // via default ctor which leaves their List<int>/Dictionary fields null, so the
            // subsequent AddRange(...) throws. This test pins current behavior.
            var deck = new Deck("D")
            {
                MainDeck = new List<Card>
                {
                    new Card { KonamiCardId = 10 },
                    new Card { KonamiCardId = 20 },
                    new Card { KonamiCardId = 30 }
                },
                ExtraDeck = new List<Card> { new Card { KonamiCardId = 40 } },
                SideDeck = new List<Card> { new Card { KonamiCardId = 50 } }
            };

            Assert.Throws<NullReferenceException>(() => DeckFormatConverter.ConvertYdkToJson(deck));
        }

        [Fact]
        public void ConvertJsonToYdk_ReturnsEmptyDeck()
        {
            var json = new JsonDeck { Name = "any" };
            var result = DeckFormatConverter.ConvertJsonToYdk(json);
            Assert.NotNull(result);
            Assert.Null(result.DeckName);
        }

        [Fact]
        public void WriteJsonFile_ReadJsonFile_RoundTrip()
        {
            const string payload = "{\"name\":\"x\"}";

            DeckFormatConverter.WriteJsonFile(_tempFile, payload);
            var readBack = DeckFormatConverter.ReadJsonFile(_tempFile);

            Assert.Equal(payload, readBack);
        }
    }
}
