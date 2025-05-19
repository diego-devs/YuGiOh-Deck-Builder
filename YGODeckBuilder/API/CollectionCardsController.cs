using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using System.Linq;

namespace YGODeckBuilder.API
{
    [ApiController]
    [Route("api/collections/{collectionId}/[controller]")]
    public class CollectionCardsController : ControllerBase
    {
        private readonly YgoContext _context;

        public CollectionCardsController(YgoContext context)
        {
            _context = context;
        }

        // GET: api/collections/{collectionId}/collectioncards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionCard>>> GetCollectionCards(int collectionId)
        {
            var collectionCards = await _context.CollectionCards
                .Where(cc => cc.CollectionId == collectionId)
                .Include(cc => cc.Card)
                .ToListAsync();

            return collectionCards;
        }

        // GET: api/collections/{collectionId}/collectioncards/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionCard>> GetCollectionCard(int collectionId, int id)
        {
            var collectionCard = await _context.CollectionCards
                .Include(cc => cc.Card)
                .FirstOrDefaultAsync(cc => cc.CollectionCardId == id && cc.CollectionId == collectionId);

            if (collectionCard == null)
            {
                return NotFound();
            }

            return collectionCard;
        }

        // POST: api/collections/{collectionId}/collectioncards
        [HttpPost]
        public async Task<ActionResult<CollectionCard>> CreateCollectionCard(int collectionId, CollectionCard collectionCard)
        {
            // Ensure the collection card's CollectionId matches the route
            collectionCard.CollectionId = collectionId;

            _context.CollectionCards.Add(collectionCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCollectionCard),
                new { collectionId = collectionId, id = collectionCard.CollectionCardId },
                collectionCard);
        }

        // PUT: api/collections/{collectionId}/collectioncards/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollectionCard(int collectionId, int id, CollectionCard collectionCard)
        {
            if (id != collectionCard.CollectionCardId || collectionId != collectionCard.CollectionId)
            {
                return BadRequest();
            }

            _context.Entry(collectionCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CollectionCards.Any(cc => cc.CollectionCardId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/collections/{collectionId}/collectioncards/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectionCard(int collectionId, int id)
        {
            var collectionCard = await _context.CollectionCards
                .FirstOrDefaultAsync(cc => cc.CollectionCardId == id && cc.CollectionId == collectionId);

            if (collectionCard == null)
            {
                return NotFound();
            }

            _context.CollectionCards.Remove(collectionCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
