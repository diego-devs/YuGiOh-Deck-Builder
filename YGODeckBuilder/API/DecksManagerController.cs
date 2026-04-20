using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Interfaces;
using YGODeckBuilder.Pages;

namespace YGODeckBuilder.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/decks")]
    [ApiController]
    public class DecksManagerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDeckUtility _deckUtility;
        private YgoContext _ygoContext;
        private readonly ILogger<DecksManagerController> _logger;

        public DecksManagerController(IConfiguration configuration, YgoContext ygoContext, IDeckUtility deckUtility, ILogger<DecksManagerController> logger)
        {
            this._configuration = configuration;
            _ygoContext = ygoContext;
            _deckUtility = deckUtility;
            _logger = logger;
        }

        private static string SanitizeDeckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            if (name.Length > 100) return null;
            if (name.Contains("..") || name.Contains('/') || name.Contains('\\')) return null;
            foreach (char c in name)
            {
                if (!char.IsLetterOrDigit(c) && c != ' ' && c != '-' && c != '_')
                    return null;
            }
            return name;
        }

        [HttpPost("save")]
        public IActionResult SaveDeck([FromBody] Deck deck)
        {
            _logger.LogInformation("Saving deck {DeckName}.ydk", deck.DeckName);

            if (SanitizeDeckName(deck.DeckName) == null)
                return BadRequest("Invalid deck name.");

            if (deck.MainDeck != null)
            {
                try
                {
                    _deckUtility.ExportDeck(deck);
                    RecordDeck(deck);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error saving deck {DeckName}", deck.DeckName);
                    return new BadRequestResult();
                }
                return new OkResult();
            }
            return StatusCode(500, "Failed to save deck");
        }

        private void RecordDeck(Deck deck)
        {
            _ygoContext.Decks.Add(deck);
            _ygoContext.SaveChanges();
        }


        [HttpPost("upload")]
        public async Task<Deck> UploadDeck([FromBody] string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new BadHttpRequestException("Path cannot be empty");
            if (Path.GetExtension(path) != ".ydk")
                throw new BadHttpRequestException("File must be a .ydk file");

            var deckName = Path.GetFileNameWithoutExtension(path);
            if (SanitizeDeckName(deckName) == null)
                throw new BadHttpRequestException("Invalid deck name.");

            var destinationPath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");

            if (System.IO.File.Exists(destinationPath))
            {
                _logger.LogError("Error duplicating deck {DeckName}.ydk: Destination file already exists", path);
                throw new BadHttpRequestException($"Deck {Path.GetFileName(path)} already exists");
            }

            System.IO.File.Copy(path, destinationPath);
            var deck = await _deckUtility.LoadDeckAsync(destinationPath);
            deck.DeckName = deckName;

            RecordDeck(deck);

            _logger.LogInformation("Deck {DeckName}.ydk duplicated to {DestinationPath} successfully", deckName, destinationPath);

            return deck;
        }

        [HttpPost("duplicate")]
        public IActionResult DuplicateDeck([FromBody] string deckName)
        {
            if (SanitizeDeckName(deckName) == null)
                return BadRequest("Invalid deck name.");

            try
            {
                string originPath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");
                string destinationPath = Path.Combine(_configuration["Paths:DecksFolderPath"], "Copy_" + deckName + ".ydk");

                if (System.IO.File.Exists(destinationPath))
                {
                    _logger.LogError("Error duplicating deck {DeckName}.ydk: Destination file already exists", deckName);
                    return new ConflictResult();
                }

                System.IO.File.Copy(originPath, destinationPath);

                _logger.LogInformation("Deck {DeckName}.ydk duplicated to {DestinationPath} successfully", deckName, destinationPath);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error duplicating deck {DeckName}.ydk", deckName);
                return new BadRequestResult();
            }
        }


        [HttpPost("rename")]
        public IActionResult RenameDeck([FromBody] RenameDeckRequest request)
        {
            if (SanitizeDeckName(request.OldDeckName) == null || SanitizeDeckName(request.NewDeckName) == null)
                return BadRequest("Invalid deck name.");

            try
            {
                string oldDeckFilePath = Path.Combine(_configuration["Paths:DecksFolderPath"], request.OldDeckName + ".ydk");
                string newDeckFilePath = Path.Combine(_configuration["Paths:DecksFolderPath"], request.NewDeckName + ".ydk");

                System.IO.File.Move(oldDeckFilePath, newDeckFilePath);

                _logger.LogInformation("Deck {OldDeckName}.ydk renamed to {NewDeckName}.ydk successfully", request.OldDeckName, request.NewDeckName);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error renaming deck {OldDeckName}.ydk", request.OldDeckName);
                return new BadRequestResult();
            }
        }

        [HttpPost("delete")]
        public IActionResult DeleteDeck([FromBody] string deckName)
        {
            if (SanitizeDeckName(deckName) == null)
                return BadRequest("Invalid deck name.");

            try
            {
                string deckFilePath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");
                System.IO.File.Delete(deckFilePath);

                _logger.LogInformation("Deck {DeckName}.ydk deleted successfully", deckName);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting deck {DeckName}.ydk", deckName);
                return new BadRequestResult();
            }
        }

        [HttpPost("new")]
        public IActionResult CreateDeck([FromBody] string name)
        {
            if (SanitizeDeckName(name) == null)
                return BadRequest("Invalid deck name.");

            string deckName = name;
            Deck newDeck = new Deck();
            newDeck.DeckName = deckName;

            try
            {
                _deckUtility.ExportDeck(newDeck);

                return RedirectToPage("/DeckBuilder", new { DeckFileName = newDeck.DeckName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create deck {DeckName}", deckName);
                return new ContentResult
                {
                    Content = $"Failed to create the new deck {ex.Message}",
                    ContentType = "text/plain",
                    StatusCode = 500
                };
            }
        }
    }

    public class RenameDeckRequest
    {
        [JsonPropertyName("oldDeckName")]
        public string OldDeckName { get; set; }
        [JsonPropertyName("newDeckName")]
        public string NewDeckName { get; set; }
    }
}
