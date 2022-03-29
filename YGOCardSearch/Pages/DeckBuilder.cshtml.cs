using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using YGOCardSearch.Models;
using YGOCardSearch.DataProviders;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using YGOCardSearch.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace YGOCardSearch.Pages
{
    public class DeckBuilder : PageModel
    {
        // Esto también debería cambiar por db:
        public List<DeckModel> LoadedDecks;
        
        // Deck a visualizar
        public DeckModel Deck { get; set; } 

        // Repo de todas las cartas (migrar a db) 
        public List<CardModel> AllCards { get; set; }



        private readonly YgoContext ygoContext;

        public DeckBuilder(YgoContext context)
        {
            ygoContext = context;
        }




        public async Task<IActionResult> OnGet()
        {
            // Good place to initialize data ?
            AllCards = LoadAllCards();
            using (var context = new YgoContext())
            {
                context.AddRange(AllCards);

            }
                //  Load a local deck file
                string path = @"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\Decks\deck1.ydk";
            Deck = LoadDeck(path);
            LoadedDecks.Add(Deck);
            return Page();

        }
        public List<CardModel> LoadAllCards() 
        {
            // Carga todas las cartas de un json
            var allCardsPath = @"C:\Users\d_dia\source\repos\YuGiOhTCG\YGOCardSearch\DataLayer\allCards.txt";
            var jsonCards = System.IO.File.ReadAllText(allCardsPath);
            var AllCards = JsonSerializer.Deserialize<List<CardModel>>(jsonCards);
            return AllCards;
        }
        /// <summary>
        /// Revisa si el deck no tiene ningún error, de lo contrario lo corrige y regresa el deck.
        /// </summary>
        /// <param name="deckList"></param>
        public static List<string> CleanDeck(List<string> deckList) 
        {
            var returnedDeckList = new List<string>(deckList);

            foreach (var cardId in deckList)
            {
                var cardChars = cardId.ToCharArray();
                bool allClean = cardChars.All(c => char.IsDigit(c));
                if (allClean) 
                {
                    continue;
                }
                else 
                {
                    var onlyDigitsIds = cardChars.Where(c => char.IsDigit(c));
                    var stringId = new string(onlyDigitsIds.ToArray());
                    
                    returnedDeckList.Add(stringId);
                    returnedDeckList.Remove(cardId);
                    continue;
                }
               
            }
            
            return returnedDeckList;
        } 
        ///<summary>
        ///<para>Regresa un DeckModel tomando un archivo .ydk como parámetro</para>
        ///<para>archivos .ydk</para>
        ///</summary>
        public DeckModel LoadDeck(string path)
        {
            // Asegurarnos que la extensión sea .ydk (?)...
            string[] ydkDeck = System.IO.File.ReadAllLines(path); // De donde sea se encuentren los decks? 
            List<string> deckIds = new List<string>(ydkDeck);

            int mainIndex = deckIds.FindIndex(c => c.Contains("#main"));
            int extraIndex = deckIds.FindIndex(r => r.Contains("#extra"));
            int sideIndex = deckIds.FindIndex(c => c.Contains("!side"));
            
            var mainDeckResult = deckIds.Skip(mainIndex + 1).Take(extraIndex - (mainIndex + 1)).ToList();
            var extraDeckResult = deckIds.Skip(extraIndex + 1).Take(sideIndex - (extraIndex + 1)).ToList();
            var sideDeckResult = deckIds.Skip(sideIndex + 1).Take(sideIndex - (extraIndex + 1)).ToList();

            // Limpiar las listas de IDS, algunos decks podrían tener errores como: "112345f" 
            var cleanedMain = CleanDeck(mainDeckResult);
            var cleanedExtra = CleanDeck(extraDeckResult);
            var cleanedSide = CleanDeck(sideDeckResult);
            
            // Obtener todas las cartas de las listas de Ids a listas de cartas
            var mainDeck = new List<CardModel>(getCards(cleanedMain));
            var extraDeck = new List<CardModel>(getCards(cleanedExtra));
            var sideDeck = new List<CardModel>(getCards(cleanedSide));

            // Finalmente crear el deck retornado
            var newDeck = new DeckModel();
            newDeck.MainDeck = mainDeck;
            newDeck.ExtraDeck = extraDeck;
            newDeck.SideDeck = sideDeck;
            newDeck.DeckName = mainDeck[0].Name.ToLower();
            return newDeck;

        }
        /// <summary>
        /// Regresa una lista de cartas del modelo apartir de una lista de strings
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public List<CardModel> getCards(List<string> cardList) 
        {
            var returnList = new List<CardModel>();
            foreach (var id in cardList)
            {
                var card = AllCards.Single(c => c.Id == Int32.Parse(id));
                returnList.Add(card);
            }
            return returnList;
        }
        
    }
}
