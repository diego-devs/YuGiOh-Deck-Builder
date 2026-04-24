using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder.Pages
{
    public class DecksManager : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IDeckUtility _deckUtility;
        public readonly YgoContext Context;

        public List<DeckPreview> PersonalDecks  { get; set; } = [];
        public List<DeckPreview> CommunityDecks { get; set; } = [];
        public bool IsAuthenticated => User.Identity?.IsAuthenticated == true;

        public DecksManager(YgoContext context, IConfiguration configuration, IDeckUtility deckUtility)
        {
            Context = context;
            _configuration = configuration;
            _deckUtility = deckUtility;
        }

        public async Task OnGetAsync()
        {
            int? userId = null;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out var uid)) userId = uid;

            var result = await Task.Run(() => _deckUtility.LoadDecksPreview(userId));
            PersonalDecks  = result.PersonalDecks;
            CommunityDecks = result.CommunityDecks;
        }
    }
}
