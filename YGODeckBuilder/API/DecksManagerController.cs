using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
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

        [HttpPost("save")]
        [Authorize]
        public IActionResult SaveDeck([FromBody] Deck deck)
        {
            _logger.LogInformation("Saving deck {DeckName}.ydk", deck.DeckName);

            if (DeckUtility.SanitizeDeckName(deck.DeckName) == null)
                return BadRequest("Invalid deck name.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            if (deck.MainDeck != null)
            {
                try
                {
                    var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
                    var deckFilePath = Path.Combine(userFolder, deck.DeckName + ".ydk");

                    if (!DeckUtility.IsPathSafe(userFolder, deckFilePath))
                        return BadRequest("Invalid path.");

                    _deckUtility.ExportDeck(deck, deckFilePath);
                    RecordDeck(deck, userId.Value);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error saving deck {DeckName}", deck.DeckName);
                    return BadRequest(e.Message);
                }
                return new OkResult();
            }
            return StatusCode(500, "Failed to save deck");
        }

        private void RecordDeck(Deck deck, int userId)
        {
            // Only persist deck metadata — the .ydk file is the source of truth for card contents.
            var existing = _ygoContext.Decks.FirstOrDefault(d => d.DeckName == deck.DeckName && d.UserId == userId);
            if (existing == null)
                _ygoContext.Decks.Add(new Deck { DeckName = deck.DeckName, UserId = userId });

            _ygoContext.SaveChanges();
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<Deck> UploadDeck([FromBody] string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new BadHttpRequestException("Path cannot be empty");
            if (Path.GetExtension(path) != ".ydk")
                throw new BadHttpRequestException("File must be a .ydk file");

            var userId = GetUserId();
            if (userId == null) throw new BadHttpRequestException("Not authenticated");

            var deckName = Path.GetFileNameWithoutExtension(path);
            if (DeckUtility.SanitizeDeckName(deckName) == null)
                throw new BadHttpRequestException("Invalid deck name.");

            var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
            var destinationPath = Path.Combine(userFolder, deckName + ".ydk");

            if (!DeckUtility.IsPathSafe(userFolder, destinationPath))
                throw new BadHttpRequestException("Invalid path.");

            if (System.IO.File.Exists(destinationPath))
            {
                _logger.LogError("Error uploading deck {DeckName}.ydk: Destination file already exists", path);
                throw new BadHttpRequestException($"Deck {Path.GetFileName(path)} already exists");
            }

            System.IO.File.Copy(path, destinationPath);
            var deck = await _deckUtility.LoadDeckAsync(destinationPath);
            deck.DeckName = deckName;

            RecordDeck(deck, userId.Value);
            _logger.LogInformation("Deck {DeckName}.ydk uploaded successfully", deckName);

            return deck;
        }

        [HttpPost("fork")]
        [Authorize]
        public IActionResult ForkDeck([FromBody] string deckName)
        {
            if (DeckUtility.SanitizeDeckName(deckName) == null)
                return BadRequest("Invalid deck name.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var communityPath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");
                if (!DeckUtility.IsPathSafe(_configuration["Paths:DecksFolderPath"], communityPath))
                    return BadRequest("Invalid path.");
                if (!System.IO.File.Exists(communityPath))
                    return NotFound("Community deck not found.");

                var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
                var destName = deckName;
                var destPath = Path.Combine(userFolder, destName + ".ydk");

                // Avoid overwriting an existing personal deck with the same name
                int suffix = 1;
                while (System.IO.File.Exists(destPath))
                {
                    destName = $"{deckName}_{suffix++}";
                    destPath = Path.Combine(userFolder, destName + ".ydk");
                }

                System.IO.File.Copy(communityPath, destPath);
                _ygoContext.Decks.Add(new Deck { DeckName = destName, UserId = userId.Value });
                _ygoContext.SaveChanges();

                _logger.LogInformation("User {UserId} forked community deck {DeckName} as {DestName}", userId, deckName, destName);
                return Ok(new { deckName = destName });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error forking deck {DeckName}", deckName);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("duplicate")]
        [Authorize]
        public IActionResult DuplicateDeck([FromBody] string deckName)
        {
            if (DeckUtility.SanitizeDeckName(deckName) == null)
                return BadRequest("Invalid deck name.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
                string originPath      = Path.Combine(userFolder, deckName + ".ydk");
                string destinationPath = Path.Combine(userFolder, "Copy_" + deckName + ".ydk");

                if (!DeckUtility.IsPathSafe(userFolder, originPath) ||
                    !DeckUtility.IsPathSafe(userFolder, destinationPath))
                    return BadRequest("Invalid path.");

                if (System.IO.File.Exists(destinationPath))
                    return new ConflictResult();

                System.IO.File.Copy(originPath, destinationPath);
                _ygoContext.Decks.Add(new Deck { DeckName = "Copy_" + deckName, UserId = userId.Value });
                _ygoContext.SaveChanges();

                _logger.LogInformation("Deck {DeckName}.ydk duplicated successfully", deckName);
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error duplicating deck {DeckName}.ydk", deckName);
                return new BadRequestResult();
            }
        }

        [HttpPost("rename")]
        [Authorize]
        public IActionResult RenameDeck([FromBody] RenameDeckRequest request)
        {
            if (DeckUtility.SanitizeDeckName(request.OldDeckName) == null || DeckUtility.SanitizeDeckName(request.NewDeckName) == null)
                return BadRequest("Invalid deck name.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
                string oldPath = Path.Combine(userFolder, request.OldDeckName + ".ydk");
                string newPath = Path.Combine(userFolder, request.NewDeckName + ".ydk");

                if (!DeckUtility.IsPathSafe(userFolder, oldPath) ||
                    !DeckUtility.IsPathSafe(userFolder, newPath))
                    return BadRequest("Invalid path.");

                System.IO.File.Move(oldPath, newPath);

                var record = _ygoContext.Decks.FirstOrDefault(d => d.DeckName == request.OldDeckName && d.UserId == userId.Value);
                if (record != null) record.DeckName = request.NewDeckName;
                _ygoContext.SaveChanges();

                _logger.LogInformation("Deck {OldDeckName}.ydk renamed to {NewDeckName}.ydk", request.OldDeckName, request.NewDeckName);
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error renaming deck {OldDeckName}.ydk", request.OldDeckName);
                return new BadRequestResult();
            }
        }

        [HttpPost("delete")]
        [Authorize]
        public IActionResult DeleteDeck([FromBody] string deckName)
        {
            if (DeckUtility.SanitizeDeckName(deckName) == null)
                return BadRequest("Invalid deck name.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
                string deckFilePath = Path.Combine(userFolder, deckName + ".ydk");

                if (!DeckUtility.IsPathSafe(userFolder, deckFilePath))
                    return BadRequest("Invalid path.");

                System.IO.File.Delete(deckFilePath);

                var record = _ygoContext.Decks.FirstOrDefault(d => d.DeckName == deckName && d.UserId == userId.Value);
                if (record != null) _ygoContext.Decks.Remove(record);
                _ygoContext.SaveChanges();

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
        [Authorize]
        public IActionResult CreateDeck([FromBody] string name)
        {
            if (DeckUtility.SanitizeDeckName(name) == null)
                return BadRequest("Invalid deck name.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var userFolder = _deckUtility.GetUserDecksFolderPath(userId.Value);
            var newDeck = new Deck { DeckName = name };

            try
            {
                _deckUtility.ExportDeck(newDeck, Path.Combine(userFolder, name + ".ydk"));
                _ygoContext.Decks.Add(new Deck { DeckName = name, UserId = userId.Value });
                _ygoContext.SaveChanges();

                return RedirectToPage("/DeckBuilder", new { DeckFileName = newDeck.DeckName, personal = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create deck {DeckName}", name);
                return StatusCode(500, "Failed to create deck.");
            }
        }

        // -------------------------------------------------------------------------

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
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
