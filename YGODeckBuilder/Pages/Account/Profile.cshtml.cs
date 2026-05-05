using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly YgoContext _db;
        private readonly IDeckUtility _deckUtility;

        public string Username { get; set; }
        public DateTime MemberSince { get; set; }
        public int DeckCount { get; set; }
        public int FavoriteCount { get; set; }
        public List<DeckPreview> Decks { get; set; } = [];

        public ProfileModel(YgoContext db, IDeckUtility deckUtility)
        {
            _db = db;
            _deckUtility = deckUtility;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return RedirectToPage("/Account/Login");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return RedirectToPage("/Account/Login");

            Username = user.Username;
            MemberSince = user.CreatedAt;

            FavoriteCount = await _db.FavoriteCards
                .CountAsync(f => f.UserId == userId);

            var decksResult = await Task.Run(() => _deckUtility.LoadDecksPreview(userId));
            Decks = decksResult.PersonalDecks;
            DeckCount = Decks.Count;

            return Page();
        }
    }
}
