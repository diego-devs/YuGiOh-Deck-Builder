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

        public DeckConverterController(IConfiguration configuration, YgoContext ygoContext)
        {
            this._configuration = configuration;
            _ygoContext = ygoContext;
        }
        // Convert deck
        [HttpPost("convert")]
        public async Task<JsonDeck> ConvertDeck(string deckPath, bool isYdk)
        {
            // work to do here

            if (isYdk) // convert to json
            {
                var deckName = System.IO.Path.GetFileNameWithoutExtension(deckPath);
                var deck = await _deckUtility.LoadDeckAsync(deckPath);
                var jsonDeck = DeckFormatConverter.ConvertYdkToJson(deck);
                return null;
                //System.IO.File.WriteAllText(_configuration["Paths:DecksFolderPath"]);
            }
            else // convert to ydk
            {
                return null; // to do...
            } 
        }
    }
}
