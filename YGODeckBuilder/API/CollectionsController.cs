using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using System.Linq;

namespace YGODeckBuilder.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionsController : ControllerBase
    {
        private readonly YgoContext _context;

        public CollectionsController(YgoContext context)
        {
            _context = context;
        }

        // GET: api/collections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
        {
            return await _context.Collections
                .Include(c => c.CollectionCards)
                .ToListAsync();
        }

        // GET: api/collections/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Collection>> GetCollection(int id)
        {
            var collection = await _context.Collections
                .Include(c => c.CollectionCards)
                .FirstOrDefaultAsync(c => c.CollectionId == id);

            if (collection == null)
            {
                return NotFound();
            }

            return collection;
        }

        // POST: api/collections
        [HttpPost]
        public async Task<ActionResult<Collection>> CreateCollection(Collection collection)
        {
            // Set timestamps
            collection.CreationDate = DateTime.UtcNow;
            collection.LastModifiedDate = DateTime.UtcNow;

            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCollection), new { id = collection.CollectionId }, collection);
        }

        // PUT: api/collections/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollection(int id, Collection collection)
        {
            if (id != collection.CollectionId)
            {
                return BadRequest();
            }

            // Update last modified date
            collection.LastModifiedDate = DateTime.UtcNow;
            _context.Entry(collection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Collections.Any(c => c.CollectionId == id))
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

        // DELETE: api/collections/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
