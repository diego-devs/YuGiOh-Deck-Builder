using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGODeckBuilder.Data;

namespace YGODeckBuilder.Pages
{
    public class FavoritesModel : PageModel
    {
        private const string UserCookie = "_yfav";
        private readonly YgoContext _db;

        public List<FavoriteCardView> Favorites { get; set; } = [];

        public FavoritesModel(YgoContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue(UserCookie, out var userId) || string.IsNullOrEmpty(userId))
                return;

            Favorites = await _db.FavoriteCards
                .Where(f => f.UserId == userId)
                .Include(f => f.Card)
                    .ThenInclude(c => c.CardImages)
                .Include(f => f.Card)
                    .ThenInclude(c => c.CardPrices)
                .OrderByDescending(f => f.AddedAt)
                .Select(f => new FavoriteCardView
                {
                    KonamiCardId = f.Card.KonamiCardId,
                    Name         = f.Card.Name,
                    Type         = f.Card.Type,
                    Attribute    = f.Card.Attribute,
                    ImageUrl     = f.Card.CardImages.Select(i => i.ImageUrl).FirstOrDefault(),
                    Price        = f.Card.CardPrices.Select(p => p.TcgPlayer).FirstOrDefault(),
                    AddedAt      = f.AddedAt
                })
                .ToListAsync();
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
