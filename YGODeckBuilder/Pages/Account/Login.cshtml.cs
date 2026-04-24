using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;

namespace YGODeckBuilder.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly YgoContext _db;
        private readonly IPasswordHasher<UserAccount> _hasher;

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(YgoContext db, IPasswordHasher<UserAccount> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter your username and password.";
                return Page();
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == Username);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToPage("/MainPage");
        }
    }
}
