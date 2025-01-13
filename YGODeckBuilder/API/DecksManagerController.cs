using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using YGODeckBuilder.Data;
using YGODeckBuilder.Pages;

namespace YGODeckBuilder.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class DecksManagerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DeckUtility _deckUtility;
        private YgoContext _ygoContext;

        public DecksManagerController(IConfiguration configuration, YgoContext ygoContext, DeckUtility deckUtility)
        {
            this._configuration = configuration;
            _ygoContext = ygoContext;
            _deckUtility = deckUtility;
        }

        [HttpPost("save")]
        public IActionResult SaveDeck([FromBody] Deck deck)
        {
            Console.WriteLine($"Saving deck {deck.DeckName}.ydk");

            if (deck.MainDeck != null)
            {
                try
                {
                    // Save the deck as .YDK file
                    _deckUtility.ExportDeck(deck);
                    // Save the deck into database
                    RecordDeck(deck);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new BadRequestResult();
                }
                return new OkResult(); // Return a success response
            }
            return null;
        }

        private void RecordDeck(Deck deck)
        {
            // Save deck into deck table in db
            _ygoContext.Decks.Add(deck);
            _ygoContext.SaveChanges();
        }

       
        [HttpPost("load")]
        public IActionResult LoadDeck([FromBody] string path)
        {
            if (string.IsNullOrEmpty(path)) return new BadRequestResult();
            if (Path.GetExtension(path) != ".ydk") return new BadRequestResult();

            var deckName = Path.GetFileNameWithoutExtension(path);

            var destinationPath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");
            
            // record deck into database table
            //// read ydkfile into a Deck object

            if (System.IO.File.Exists(destinationPath))
            {
                Console.WriteLine($"Error duplicating deck {path}.ydk: Destination file already exists");
                return new ConflictResult(); // HTTP 409 Conflict status code
            }

            System.IO.File.Copy(path, destinationPath);

            Console.WriteLine($"Deck {deckName}.ydk duplicated to {destinationPath} successfully");

            // use that Deck oject to create a new ydk file
            // save the new ydk file into decks location

            return Ok();
        }

        [HttpPost("duplicate")]
        public IActionResult DuplicateDeck([FromBody] string deckName)
        { 
            try
            {
                string originPath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");
                string destinationPath = Path.Combine(_configuration["Paths:DecksFolderPath"], "Copy_" + deckName + ".ydk");

                if (System.IO.File.Exists(destinationPath))
                {
                    Console.WriteLine($"Error duplicating deck {deckName}.ydk: Destination file already exists");
                    return new ConflictResult(); // Return HTTP 409 Conflict status code
                }

                System.IO.File.Copy(originPath, destinationPath);

                Console.WriteLine($"Deck {deckName}.ydk duplicated to {destinationPath} successfully");

                return new OkResult(); // Return HTTP 200 OK status code
            }
            catch (Exception e)
            {
                // Handle other exceptions
                Console.WriteLine($"Error duplicating deck {deckName}.ydk: {e.Message}");
                return new BadRequestResult(); // Return HTTP 400 Bad Request status code
            }
        }


        [HttpPost("rename")]
        public IActionResult RenameDeck([FromBody] RenameDeckRequest request)
        {
            try
            {
                string oldDeckFilePath = Path.Combine(_configuration["Paths:DecksFolderPath"], request.OldDeckName + ".ydk");
                string newDeckFilePath = Path.Combine(_configuration["Paths:DecksFolderPath"], request.NewDeckName + ".ydk");

                System.IO.File.Move(oldDeckFilePath, newDeckFilePath);

                Console.WriteLine($"Deck {request.OldDeckName}.ydk renamed to {request.NewDeckName}.ydk successfully");

                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error renaming deck {request.OldDeckName}.ydk: {e.Message}");
                return new BadRequestResult();
            }
        }

        [HttpPost("delete")]
        public IActionResult DeleteDeck([FromBody] string deckName)
        {
            try
            {
                string deckFilePath = Path.Combine(_configuration["Paths:DecksFolderPath"], deckName + ".ydk");
                System.IO.File.Delete(deckFilePath);

                Console.WriteLine($"Deck {deckName}.ydk deleted successfully");

                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting deck {deckName}.ydk: {e.Message}");
                return new BadRequestResult();
            }
        }

        [HttpPost("new")]
        public IActionResult CreateDeck([FromBody] string deckName) 
        {
            if (string.IsNullOrWhiteSpace(deckName)) return BadRequest("Deck name cannot be empty.");
            
            Deck newDeck = new Deck();
            newDeck.DeckName = deckName;

            try
            {
                string decksLocalFolder = _configuration["Paths:DecksFolderPath"];
                string deckFilePath = $"{decksLocalFolder}\\{deckName}.ydk";

                ExportDeck(newDeck);

                return RedirectToPage("/DeckBuilder", new { DeckFileName = newDeck.DeckName });
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
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
