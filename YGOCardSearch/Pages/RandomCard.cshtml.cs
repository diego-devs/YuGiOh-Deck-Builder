using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YGOCardSearch.Pages
{
    public class RandomCardModel : PageModel
    {
        public void GetRandomCard() 
        {
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
