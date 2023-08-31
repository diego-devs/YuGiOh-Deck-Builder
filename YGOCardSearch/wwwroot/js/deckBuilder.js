import { Deck } from './deck.js';
import { renderDeckCards } from './rendering.js';

// Create an instance of the Deck class
const deck = new Deck(deckData);
console.log(deck.deck_name);
console.log(deck.mainDeck.Lenght);
renderDeck(deck);

// Function to render the entire deck
function renderDeck() {
    const mainDeckContainer = '.DeckBuilder_Container_MainDeck';
    const extraDeckContainer = '.DeckBuilder_Container_ExtraDeck';
    const sideDeckContainer = '.DeckBuilder_Container_SideDeck';

    renderDeckCards(deck.getMainDeck(), mainDeckContainer);
    renderDeckCards(deck.getExtraDeck(), extraDeckContainer);
    renderDeckCards(deck.getSideDeck(), sideDeckContainer);
}



//// deckBuilder.js
//var DeckManager = (function () {
//    var deck = {
//        MainDeck: [],
//        ExtraDeck: [],
//        SideDeck: []
//    };

//    function addCardToDeck(card, deckSection) {
//        deck[deckSection].push(card);
//        // Call a rendering function here to update the displayed cards
//    }

//    function removeCardFromDeck(card, deckSection) {
//        var index = deck[deckSection].indexOf(card);
//        if (index !== -1) {
//            deck[deckSection].splice(index, 1);
//            // Call a rendering function here to update the displayed cards
//        }
//    }

//    return {
//        addCardToDeck: addCardToDeck,
//        removeCardFromDeck: removeCardFromDeck
//    };
//})();
