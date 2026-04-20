using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.JsonFormatDeck;

namespace YGODeckBuilder.API
{
    [Route("api/converter")]
    [ApiController]
    public class DeckConverterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DeckUtility _deckUtility;
        private YgoContext _ygoContext;

        public DeckConverterController(IConfiguration configuration, YgoContext ygoContext, DeckUtility deckUtility)
        {
            _configuration = configuration;
            _ygoContext = ygoContext;
            _deckUtility = deckUtility;
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

        // Convert deck
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertDeck(string deckPath, bool isYdk)
        {
            var deckFileName = Path.GetFileNameWithoutExtension(deckPath);
            if (SanitizeDeckName(deckFileName) == null)
                return BadRequest("Invalid deck name.");

            try
            {
                string outputPath = Path.Combine(_configuration["Paths:DecksFolderPath"],
                    deckFileName);

                if (isYdk) // convert YDK to JSON
                {
                    var deck = await _deckUtility.LoadDeckAsync(deckPath);
                    var jsonDeck = DeckFormatConverter.ConvertYdkToJson(deck);
                    string jsonOutputPath = outputPath + ".json";
                    await System.IO.File.WriteAllTextAsync(jsonOutputPath, System.Text.Json.JsonSerializer.Serialize(jsonDeck));
                    return Ok(jsonDeck);
                }
                else // convert JSON to YDK
                {
                    var jsonContent = await System.IO.File.ReadAllTextAsync(deckPath);
                    var jsonDeck = System.Text.Json.JsonSerializer.Deserialize<JsonDeck>(jsonContent);
                    var ydkDeck = DeckFormatConverter.ConvertJsonToYdk(jsonDeck);
                    string ydkOutputPath = outputPath + ".ydk";
                    _deckUtility.ExportDeck(ydkDeck, ydkOutputPath);

                    
                    return Ok(ydkDeck);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting deck: {ex.Message}");
            }
        }
    }
}
