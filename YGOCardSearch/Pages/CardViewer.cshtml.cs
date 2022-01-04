using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGOCardSearch.Models;

namespace YGOCardSearch.Pages
{
    public class CardViewerModel : PageModel
    {
        public void OnGet()
        {
            public ICardProdiver cardProvider { get; set; }
            public List<ygoModel> Movies { get; set; }
            public ygoModel Card { get; set; }

            public MoviesModel(IMoviesProvider moviesProvider)
            {
                this.cardProvider = moviesProvider;
            }

            public async Task<IActionResult> OnGet()
            {
                var popularMovies = await cardProvider.GetAllAsync();
                if (popularMovies != null)
                {
                    Movies = new List<Movie>(popularMovies);
                }
                return Page();


            }
        
    }
}
