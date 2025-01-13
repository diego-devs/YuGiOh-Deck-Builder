using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/cards")]
    [ApiController]
    public class CardsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DeckUtility _deckUtility;
        private YgoContext _ygoContext;

        public CardsController(IConfiguration configuration, YgoContext ygoContext)
        {
            this._configuration = configuration;
            _ygoContext = ygoContext;
        }

        // GET: cards/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCardAsync(int id)
        {
            var card = await _ygoContext.Cards.FirstOrDefaultAsync(c => c.CardId == id);
            if (card == null)
                return NotFound();
                
            return Ok(card);
        }

        // GET: cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsAsync([FromQuery] int? id = null)
        {
            var query = _ygoContext.Cards.AsQueryable();
            
            if (id.HasValue)
                query = query.Where(c => c.CardId == id);
                
            var cards = await query.ToListAsync();
            if (!cards.Any())
                return NotFound();
                
            return Ok(cards);
        }

        // GET: search of cards
        // GET: search of cards with filters

    }
}
