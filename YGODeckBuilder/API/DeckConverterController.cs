using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.JsonFormatDeck;

namespace YGODeckBuilder.API
{
    [Route("api/[controller]")]
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
        // Convert deck
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertDeck(string deckPath, bool isYdk)
        {
            try
            {
                string outputPath = Path.Combine(_configuration["Paths:DecksFolderPath"], 
                    Path.GetFileNameWithoutExtension(deckPath));

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
                    await _deckUtility.ExportDeck(ydkDeck, ydkOutputPath);
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
