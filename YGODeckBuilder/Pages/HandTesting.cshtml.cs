using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.Models;
using System.IO;

namespace YGODeckBuilder.Pages
{

    public class HandTestingModel : PageModel
    {
        private readonly DeckUtility deckUtility;
        private readonly IConfiguration _configuration;
        public Deck Deck { get; set; }
        public List<Card> CurrentHand { get; set; } = new List<Card>();

        public HandTestingModel(YgoContext db, IConfiguration config)
        {
            deckUtility = new DeckUtility(db, config);
            _configuration = config;
        }

        public async void OnGet(string deckName)
        {
            if (string.IsNullOrEmpty(deckName))
            {
                // Handle error: no deck name provided
                return;
            }

            // Get the path to the .ydk file
            string decksFolderPath = _configuration["DecksFolderPath"];
            string deckFilePath = Path.Combine(decksFolderPath, $"{deckName}.ydk");

            if (!System.IO.File.Exists(deckFilePath))
            {
                // Handle error: deck file not found
                return;
            }

            // Load the deck from the .ydk file
            Deck = await deckUtility.LoadDeckAsync(deckFilePath);

            if (Deck == null)
            {
                // Handle error: failed to load deck
                return;
            }

            deckUtility.Deck = Deck;
            deckUtility.ShuffleDeck();
            DrawInitialHand();
        }

        private void InitializeDeck(string deckJson)
        {
            if (!string.IsNullOrEmpty(deckJson))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Deck = JsonSerializer.Deserialize<Deck>(deckJson, options);

                    if (Deck == null)
                    {
                        throw new JsonException("Deserialization resulted in a null Deck object.");
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error and create a new empty deck
                    Console.WriteLine($"Error deserializing deck: {ex.Message}");
                    Deck = new Deck();
                }
            }
            else
            {
                Deck = new Deck();
            }

            deckUtility.Deck = Deck;
            deckUtility.ShuffleDeck();
        }

        private void DrawInitialHand(int cardCount = 5)
        {
            CurrentHand = deckUtility.DrawCards(cardCount);
        }

        public IActionResult OnPostDrawCard()
        {
            var drawnCard = deckUtility.DrawCards(1).FirstOrDefault();
            if (drawnCard != null)
            {
                CurrentHand.Add(drawnCard);
            }
            return Page();
        }

        public IActionResult OnPostShuffleDeck()
        {
            deckUtility.ShuffleDeck();
            return Page();
        }
    }
}
