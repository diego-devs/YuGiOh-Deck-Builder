using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.API
{
    [Route("api/favorites")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly YgoContext _db;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(YgoContext db, ILogger<FavoritesController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET api/favorites
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = GetUserId();

            var favorites = await _db.FavoriteCards
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedAt)
                .Select(f => new { f.FavoriteId, f.AddedAt, f.KonamiCardId })
                .ToListAsync();

            var konamiIds = favorites.Select(f => f.KonamiCardId).ToList();

            var cards = await _db.Cards
                .Where(c => konamiIds.Contains(c.KonamiCardId))
                .Select(c => new { c.KonamiCardId, c.Name, c.Type, c.Desc, c.Atk, c.Def, c.Level, c.Race, c.Attribute })
                .ToDictionaryAsync(c => c.KonamiCardId);

            var images = await _db.CardImages
                .Where(i => konamiIds.Contains(i.CardImageId))
                .GroupBy(i => i.CardImageId)
                .Select(g => new { KonamiCardId = g.Key, ImageUrl = g.First().ImageUrl })
                .ToDictionaryAsync(x => x.KonamiCardId);

            var result = favorites.Select(f =>
            {
                cards.TryGetValue(f.KonamiCardId, out var card);
                images.TryGetValue(f.KonamiCardId, out var img);
                return new
                {
                    f.FavoriteId,
                    f.AddedAt,
                    card = new
                    {
                        f.KonamiCardId,
                        Name = card?.Name,
                        Type = card?.Type,
                        Desc = card?.Desc,
                        Atk = card?.Atk,
                        Def = card?.Def,
                        Level = card?.Level,
                        Race = card?.Race,
                        Attribute = card?.Attribute,
                        imageUrl = img?.ImageUrl
                    }
                };
            }).ToList();

            return Ok(result);
        }

        // GET api/favorites/{konamiCardId}/status
        [HttpGet("{konamiCardId}/status")]
        public async Task<IActionResult> GetStatus(int konamiCardId)
        {
            var userId = GetUserId();
            var isFavorited = await _db.FavoriteCards
                .AnyAsync(f => f.UserId == userId && f.KonamiCardId == konamiCardId);
            return Ok(new { isFavorited });
        }

        // POST api/favorites  { "konamiCardId": 12345 }
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteRequest request)
        {
            if (request.KonamiCardId <= 0)
                return BadRequest("Invalid card ID.");

            var userId = GetUserId();

            var alreadyExists = await _db.FavoriteCards
                .AnyAsync(f => f.UserId == userId && f.KonamiCardId == request.KonamiCardId);

            if (alreadyExists)
                return Conflict("Card is already in favorites.");

            _db.FavoriteCards.Add(new FavoriteCard
            {
                KonamiCardId = request.KonamiCardId,
                UserId       = userId,
                AddedAt      = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} added card {KonamiCardId} to favorites", userId, request.KonamiCardId);

            return Ok(new { isFavorited = true });
        }

        // DELETE api/favorites/{konamiCardId}
        [HttpDelete("{konamiCardId}")]
        public async Task<IActionResult> RemoveFavorite(int konamiCardId)
        {
            var userId = GetUserId();

            var favorite = await _db.FavoriteCards
                .FirstOrDefaultAsync(f => f.UserId == userId && f.KonamiCardId == konamiCardId);

            if (favorite == null)
                return NotFound("Favorite not found.");

            _db.FavoriteCards.Remove(favorite);
            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} removed card {KonamiCardId} from favorites", userId, konamiCardId);

            return Ok(new { isFavorited = false });
        }

        // -------------------------------------------------------------------------

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public class FavoriteRequest
    {
        [JsonPropertyName("konamiCardId")]
        public int KonamiCardId { get; set; }
    }
}
