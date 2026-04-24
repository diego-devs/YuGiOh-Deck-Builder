using System;
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
    public class RegisterModel : PageModel
    {
        private readonly YgoContext _db;
        private readonly IPasswordHasher<UserAccount> _hasher;

        [BindProperty] public string Username     { get; set; }
        [BindProperty] public string Password     { get; set; }
        [BindProperty] public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }

        public RegisterModel(YgoContext db, IPasswordHasher<UserAccount> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Username.Length > 50)
            {
                ErrorMessage = "Username must be between 3 and 50 characters.";
                return Page();
            }
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters.";
                return Page();
            }
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            var exists = await _db.Users.AnyAsync(u => u.Username == Username);
            if (exists)
            {
                ErrorMessage = "That username is already taken.";
                return Page();
            }

            var user = new UserAccount
            {
                Username  = Username,
                CreatedAt = DateTime.UtcNow
            };
            user.PasswordHash = _hasher.HashPassword(user, Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            await SignInAsync(user);
            return RedirectToPage("/MainPage");
        }

        private async Task SignInAsync(UserAccount user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
