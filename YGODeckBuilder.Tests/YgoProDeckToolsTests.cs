using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using YGODeckBuilder.Data.EntityModels;
using YugiohDB;

namespace YGODeckBuilder.Tests
{
    public class YgoProDeckToolsTests : IDisposable
    {
        private readonly string _tempDir;

        public YgoProDeckToolsTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ygodb_tests_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, recursive: true);
        }

        [Fact]
        public void SaveCardsFile_ReadAllCards_RoundTrip()
        {
            var path = Path.Combine(_tempDir, "cards.json");
            var cards = new List<Card>
            {
                new Card { CardId = 1, KonamiCardId = 100, Name = "A", Desc = "desc A", Type = "Monster" },
                new Card { CardId = 2, KonamiCardId = 200, Name = "B", Desc = "desc B", Type = "Spell" }
            };

            YgoProDeckTools.SaveCardsFile(cards, path);
            var round = YgoProDeckTools.ReadAllCards(path);

            Assert.Equal(2, round.Count);
            Assert.Equal("A", round[0].Name);
            Assert.Equal(200, round[1].KonamiCardId);
        }

        [Fact]
        public void MapImages_SetsLocalPathsForAllImages()
        {
            var cards = new List<Card>
            {
                new Card
                {
                    KonamiCardId = 1,
                    CardImages = new List<CardImages>
                    {
                        new CardImages { CardImageId = 1001 },
                        new CardImages { CardImageId = 1002 }
                    }
                },
                new Card { KonamiCardId = 2, CardImages = null }
            };

            YgoProDeckTools.MapImages(cards, "images");

            var first = cards[0].CardImages[0];
            Assert.Equal("images/1001.jpg", first.ImageLocalUrl);
            Assert.Equal("images/small/1001.jpg", first.ImageLocalUrlSmall);
            Assert.Equal("images/cropped/1001.jpg", first.ImageLocalUrlCropped);

            var second = cards[0].CardImages[1];
            Assert.Equal("images/1002.jpg", second.ImageLocalUrl);
        }

        [Fact]
        public void MapBanlistInfo_AttachesMatchingBanlistByKonamiId()
        {
            var cards = new List<Card>
            {
                new Card { KonamiCardId = 10 },
                new Card { KonamiCardId = 20 }
            };
            var banlist = new List<BanlistInfo>
            {
                new BanlistInfo { CardId = 10, Ban_TCG = "Forbidden" },
                new BanlistInfo { CardId = 999, Ban_TCG = "Limited" } // no matching card
            };

            YgoProDeckTools.MapBanlistInfo(cards, banlist);

            Assert.NotNull(cards[0].BanlistInfo);
            Assert.Equal("Forbidden", cards[0].BanlistInfo.Ban_TCG);
            Assert.Null(cards[1].BanlistInfo);
        }

        [Fact]
        public void CardImageSize_HasAllExpectedValues()
        {
            Assert.True(Enum.IsDefined(typeof(CardImageSize), CardImageSize.Small));
            Assert.True(Enum.IsDefined(typeof(CardImageSize), CardImageSize.Large));
            Assert.True(Enum.IsDefined(typeof(CardImageSize), CardImageSize.Cropped));
        }

        [Fact]
        public async Task DownloadImagesAsync_SkipsExistingNonEmptyFile()
        {
            var imagesRoot = Path.Combine(_tempDir, "imgs");
            Directory.CreateDirectory(Path.Combine(imagesRoot, "small"));
            var existing = Path.Combine(imagesRoot, "small", "1001.jpg");
            File.WriteAllText(existing, "already-downloaded");
            var originalLength = new FileInfo(existing).Length;

            var handler = new RecordingHandler(_ => throw new InvalidOperationException("HTTP should not be called for existing file"));
            var client = new HttpClient(handler);
            var cards = new List<Card>
            {
                new Card
                {
                    KonamiCardId = 1,
                    CardImages = new List<CardImages>
                    {
                        new CardImages { CardImageId = 1001, ImageUrlSmall = "http://example/img.jpg" }
                    }
                }
            };

            await YgoProDeckTools.DownloadImagesAsync(cards, CardImageSize.Small, imagesRoot, client);

            Assert.Equal(0, handler.CallCount);
            Assert.Equal(originalLength, new FileInfo(existing).Length);
        }

        [Fact]
        public async Task DownloadImagesAsync_WritesFileOnSuccess()
        {
            var imagesRoot = Path.Combine(_tempDir, "imgs2");
            var payload = Encoding.UTF8.GetBytes("image-bytes");

            var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(payload)
            });
            var client = new HttpClient(handler);

            var cards = new List<Card>
            {
                new Card
                {
                    KonamiCardId = 1,
                    CardImages = new List<CardImages>
                    {
                        new CardImages { CardImageId = 555, ImageUrl = "http://example/full.jpg" }
                    }
                }
            };

            await YgoProDeckTools.DownloadImagesAsync(cards, CardImageSize.Large, imagesRoot, client);

            var expected = Path.Combine(imagesRoot, "555.jpg");
            Assert.True(File.Exists(expected));
            Assert.Equal(payload, File.ReadAllBytes(expected));
        }

        [Fact]
        public async Task DownloadImagesAsync_SkipsEntriesWithEmptyUrl()
        {
            var imagesRoot = Path.Combine(_tempDir, "imgs3");
            var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(new byte[] { 1, 2, 3 })
            });
            var client = new HttpClient(handler);

            var cards = new List<Card>
            {
                new Card
                {
                    KonamiCardId = 1,
                    CardImages = new List<CardImages>
                    {
                        new CardImages { CardImageId = 1, ImageUrlCropped = null }
                    }
                }
            };

            await YgoProDeckTools.DownloadImagesAsync(cards, CardImageSize.Cropped, imagesRoot, client);

            Assert.Equal(0, handler.CallCount);
        }

        [Fact]
        public async Task DownloadImagesAsync_FailedDownloadDoesNotLeaveTempFile()
        {
            var imagesRoot = Path.Combine(_tempDir, "imgs4");
            var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
            var client = new HttpClient(handler);

            var cards = new List<Card>
            {
                new Card
                {
                    KonamiCardId = 1,
                    CardImages = new List<CardImages>
                    {
                        new CardImages { CardImageId = 777, ImageUrl = "http://example/fail.jpg" }
                    }
                }
            };

            await YgoProDeckTools.DownloadImagesAsync(cards, CardImageSize.Large, imagesRoot, client);

            Assert.False(File.Exists(Path.Combine(imagesRoot, "777.jpg")));
            Assert.False(File.Exists(Path.Combine(imagesRoot, "777.jpg.tmp")));
        }

        [Fact]
        public async Task DownloadImagesAsync_ThrowsOnUnknownSize()
        {
            var client = new HttpClient(new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));
            var cards = new List<Card>();
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                YgoProDeckTools.DownloadImagesAsync(cards, (CardImageSize)999, _tempDir, client));
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
            public int CallCount { get; private set; }

            public RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
            {
                _responder = responder;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CallCount++;
                return Task.FromResult(_responder(request));
            }
        }
    }
}
