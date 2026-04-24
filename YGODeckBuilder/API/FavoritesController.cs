using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.API
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private const string UserCookie = "_yfav";

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
            var userId = GetOrCreateUserId();

            var favorites = await _db.FavoriteCards
                .Where(f => f.UserId == userId)
                .Include(f => f.Card)
                    .ThenInclude(c => c.CardImages)
                .OrderByDescending(f => f.AddedAt)
                .Select(f => new
                {
                    f.FavoriteId,
                    f.AddedAt,
                    card = new
                    {
                        f.Card.CardId,
                        f.Card.KonamiCardId,
                        f.Card.Name,
                        f.Card.Type,
                        f.Card.Desc,
                        f.Card.Atk,
                        f.Card.Def,
                        f.Card.Level,
                        f.Card.Race,
                        f.Card.Attribute,
                        imageUrl = f.Card.CardImages.Select(i => i.ImageUrl).FirstOrDefault()
                    }
                })
                .ToListAsync();

            return Ok(favorites);
        }

        // GET api/favorites/{konamiCardId}/status
        [HttpGet("{konamiCardId}/status")]
        public async Task<IActionResult> GetStatus(int konamiCardId)
        {
            var userId = GetOrCreateUserId();

            var isFavorited = await _db.FavoriteCards
                .AnyAsync(f => f.UserId == userId && f.Card.KonamiCardId == konamiCardId);

            return Ok(new { isFavorited });
        }

        // POST api/favorites  { "konamiCardId": 12345 }
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteRequest request)
        {
            var userId = GetOrCreateUserId();

            var card = await _db.Cards.FirstOrDefaultAsync(c => c.KonamiCardId == request.KonamiCardId);
            if (card == null)
                return NotFound("Card not found.");

            var alreadyExists = await _db.FavoriteCards
                .AnyAsync(f => f.UserId == userId && f.CardId == card.CardId);

            if (alreadyExists)
                return Conflict("Card is already in favorites.");

            _db.FavoriteCards.Add(new FavoriteCard
            {
                CardId  = card.CardId,
                UserId  = userId,
                AddedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} added card {KonamiCardId} to favorites", userId, request.KonamiCardId);

            return Ok(new { isFavorited = true });
        }

        // DELETE api/favorites/{konamiCardId}
        [HttpDelete("{konamiCardId}")]
        public async Task<IActionResult> RemoveFavorite(int konamiCardId)
        {
            var userId = GetOrCreateUserId();

            var favorite = await _db.FavoriteCards
                .FirstOrDefaultAsync(f => f.UserId == userId && f.Card.KonamiCardId == konamiCardId);

            if (favorite == null)
                return NotFound("Favorite not found.");

            _db.FavoriteCards.Remove(favorite);
            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} removed card {KonamiCardId} from favorites", userId, konamiCardId);

            return Ok(new { isFavorited = false });
        }

        // -------------------------------------------------------------------------

        private string GetOrCreateUserId()
        {
            if (Request.Cookies.TryGetValue(UserCookie, out var existing) && !string.IsNullOrEmpty(existing))
                return existing;

            var newId = Guid.NewGuid().ToString("N");
            Response.Cookies.Append(UserCookie, newId, new CookieOptions
            {
                HttpOnly  = true,
                Secure    = true,
                SameSite  = SameSiteMode.Lax,
                Expires   = DateTimeOffset.UtcNow.AddYears(2)
            });
            return newId;
        }
    }

    public class FavoriteRequest
    {
        [JsonPropertyName("konamiCardId")]
        public int KonamiCardId { get; set; }
    }
}
