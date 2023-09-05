import { Deck } from './deck.js';
import { renderDeckCards } from './rendering.js';
import { renderSearchedCards } from './rendering.js'; // Import the renderSearchedCards function

// Create an instance of the Deck class
const deck = new Deck(deckData);
console.log(deck.deck_name);
console.log(deck.mainDeck.Lenght);
renderDeck(deck);
renderSearch(searchedCards);

// Function to render the entire deck
function renderDeck(deck) {
    const mainDeckContainer = '.DeckBuilder_Container_MainDeck';
    const extraDeckContainer = '.DeckBuilder_Container_ExtraDeck';
    const sideDeckContainer = '.DeckBuilder_Container_SideDeck';
     // Specify the container selector for searched cards here

    renderDeckCards(deck.getMainDeck(), mainDeckContainer);
    renderDeckCards(deck.getExtraDeck(), extraDeckContainer);
    renderDeckCards(deck.getSideDeck(), sideDeckContainer);

     // Call the renderSearchedCards function with your searched cards
}
function renderSearch(searchedCards) {
    renderSearchedCards(searchedCards, '.DeckBuilder_CardSearch_JS');
}



//import { Deck } from './deck.js';
//import { renderDeckCards } from './rendering.js';

//// Create an instance of the Deck class
//const deck = new Deck(deckData);
//console.log(deck.deck_name);
//console.log(deck.mainDeck.Lenght);
//renderDeck(deck);

//// Function to render the entire deck
//function renderDeck() {
//    const mainDeckContainer = '.DeckBuilder_Container_MainDeck';
//    const extraDeckContainer = '.DeckBuilder_Container_ExtraDeck';
//    const sideDeckContainer = '.DeckBuilder_Container_SideDeck';

//    renderDeckCards(deck.getMainDeck(), mainDeckContainer);
//    renderDeckCards(deck.getExtraDeck(), extraDeckContainer);
//    renderDeckCards(deck.getSideDeck(), sideDeckContainer);
//}



//// deckBuilder.js
