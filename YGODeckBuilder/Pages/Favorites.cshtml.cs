using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YGODeckBuilder.Data;

namespace YGODeckBuilder.Pages
{
    public class FavoritesModel : PageModel
    {
        private readonly YgoContext _db;

        public List<FavoriteCardView> Favorites { get; set; } = [];
        public bool IsAuthenticated => User.Identity?.IsAuthenticated == true;

        public FavoritesModel(YgoContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null || !int.TryParse(userIdStr, out var userId))
                return;

            var rows = await _db.FavoriteCards
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedAt)
                .Select(f => new { f.KonamiCardId, f.AddedAt })
                .ToListAsync();

            var ids = rows.Select(r => r.KonamiCardId).ToList();

            var cards = await _db.Cards
                .Where(c => ids.Contains(c.KonamiCardId))
                .Select(c => new { c.KonamiCardId, c.Name, c.Type, c.Attribute })
                .ToDictionaryAsync(c => c.KonamiCardId);

            var images = await _db.CardImages
                .Where(i => ids.Contains(i.CardImageId))
                .GroupBy(i => i.CardImageId)
                .Select(g => new { Id = g.Key, Url = g.First().ImageUrl })
                .ToDictionaryAsync(x => x.Id);

            var prices = await _db.CardPrices
                .Where(p => _db.Cards
                    .Where(c => ids.Contains(c.KonamiCardId))
                    .Select(c => c.CardId)
                    .Contains(p.CardId))
                .GroupBy(p => p.CardId)
                .Select(g => new { CardId = g.Key, Price = g.First().TcgPlayer })
                .ToListAsync();

            // map CardId → KonamiCardId for prices
            var cardIdToKonami = await _db.Cards
                .Where(c => ids.Contains(c.KonamiCardId))
                .Select(c => new { c.CardId, c.KonamiCardId })
                .ToDictionaryAsync(c => c.CardId, c => c.KonamiCardId);

            var priceByKonami = prices
                .Where(p => cardIdToKonami.ContainsKey(p.CardId))
                .ToDictionary(p => cardIdToKonami[p.CardId], p => p.Price);

            Favorites = rows.Select(r =>
            {
                cards.TryGetValue(r.KonamiCardId, out var card);
                images.TryGetValue(r.KonamiCardId, out var img);
                priceByKonami.TryGetValue(r.KonamiCardId, out var price);
                return new FavoriteCardView
                {
                    KonamiCardId = r.KonamiCardId,
                    Name         = card?.Name,
                    Type         = card?.Type,
                    Attribute    = card?.Attribute,
                    ImageUrl     = img?.Url,
                    Price        = price,
                    AddedAt      = r.AddedAt
                };
            }).ToList();
        }
    }

    public class FavoriteCardView
    {
        public int KonamiCardId { get; set; }
        public string Name      { get; set; }
        public string Type      { get; set; }
        public string Attribute { get; set; }
        public string ImageUrl  { get; set; }
        public string Price     { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
