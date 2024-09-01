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
        private readonly DeckUtility _deckUtility;
        private readonly IConfiguration _configuration;
        public Deck Deck { get; set; } = new Deck();
        public string ImagesFolder { get; set; }

        public HandTestingModel(YgoContext db, IConfiguration config)
        {
            _deckUtility = new DeckUtility(db, config);
            _configuration = config;
        }

        public async Task OnGetAsync(string deckName)
        {
            if (string.IsNullOrEmpty(deckName))
            {
                // Handle error: no deck name provided
                return;
            }
            ImagesFolder = _configuration["Paths:ImagesFolder"].Replace("\\", "/");
            // Get the path to the .ydk file
            string decksFolderPath = _configuration["Paths:DecksFolderPath"];
            string deckFilePath = Path.Combine(decksFolderPath, $"{deckName}.ydk");

            if (!System.IO.File.Exists(deckFilePath))
            {
                // Handle error: deck file not found
                return;
            }

            // Load the deck from the .ydk file
            Deck = await _deckUtility.LoadDeckAsync(deckFilePath);

            if (Deck == null)
            {
                // Handle error: failed to load deck
                return;
            }

            _deckUtility.Deck = Deck;
            _deckUtility.ShuffleDeck();
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

            _deckUtility.Deck = Deck;
            _deckUtility.ShuffleDeck();
        }
    }
}
