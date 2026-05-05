using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using YGODeckBuilder.Data;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder.Pages
{
    public class GameModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IDeckUtility _deckUtility;
        private readonly string _decksFolderPath;

        public Deck PlayerDeck { get; set; } = new Deck("Player");
        public Deck OpponentDeck { get; set; } = new Deck("Opponent");
        public string ImagesFolder { get; set; } = "/images";
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DeckName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OpponentDeckName { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Personal { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool OpponentPersonal { get; set; }

        public GameModel(IConfiguration config, IDeckUtility deckUtility)
        {
            _configuration = config;
            _deckUtility = deckUtility;
            _decksFolderPath = _configuration["Paths:DecksFolderPath"];
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ImagesFolder = (_configuration["Paths:ImagesFolder"] ?? "/images").Replace("\\", "/");

            if (string.IsNullOrEmpty(DeckName))
            {
                ErrorMessage = "No deck specified. Open a deck from the Decks Manager and click Play.";
                return Page();
            }

            int? userId = null;
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid)) userId = uid;

            var playerPath = ResolveDeckPath(DeckName, Personal, userId);
            if (playerPath == null || !System.IO.File.Exists(playerPath))
            {
                ErrorMessage = $"Could not find deck '{DeckName}'.";
                return Page();
            }

            PlayerDeck = await _deckUtility.LoadDeckAsync(playerPath);

            // Opponent deck: defaults to mirror match if not specified
            var opponentName = string.IsNullOrEmpty(OpponentDeckName) ? DeckName : OpponentDeckName;
            var oppPersonal = string.IsNullOrEmpty(OpponentDeckName) ? Personal : OpponentPersonal;
            var opponentPath = ResolveDeckPath(opponentName, oppPersonal, userId);
            if (opponentPath != null && System.IO.File.Exists(opponentPath))
            {
                OpponentDeck = await _deckUtility.LoadDeckAsync(opponentPath);
            }
            else
            {
                OpponentDeck = await _deckUtility.LoadDeckAsync(playerPath);
                OpponentDeck.DeckName = "Opponent (mirror)";
            }

            return Page();
        }

        private string ResolveDeckPath(string deckName, bool personal, int? userId)
        {
            var sanitized = DeckUtility.SanitizeDeckName(deckName);
            if (sanitized == null) return null;

            if (personal && userId.HasValue)
            {
                var userFolder = Path.Combine(_decksFolderPath, "users", userId.Value.ToString());
                var path = Path.Combine(userFolder, $"{sanitized}.ydk");
                if (DeckUtility.IsPathSafe(userFolder, path)) return path;
                return null;
            }

            var sharedPath = Path.Combine(_decksFolderPath, $"{sanitized}.ydk");
            if (DeckUtility.IsPathSafe(_decksFolderPath, sharedPath)) return sharedPath;
            return null;
        }
    }
}
