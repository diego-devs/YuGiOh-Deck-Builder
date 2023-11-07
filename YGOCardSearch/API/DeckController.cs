using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YGOCardSearch.Data.Models;

namespace YGOCardSearch.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class DeckController
    {
        [HttpPost("save")]
        public IActionResult SaveDeck([FromBody] Deck deck)
        {
            // Process the received deck data here, such as saving to the database or send it to create the YDK file

            return new OkResult(); // Return a success response
        }

    }
}
