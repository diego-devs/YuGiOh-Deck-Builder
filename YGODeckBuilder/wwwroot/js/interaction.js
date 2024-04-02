import { renderDeckCards} from './rendering.js'; // Import the rendering function
import {
    addCardToMainDeck,
    addCardToExtraDeck,
    addCardToSideDeck,
    removeCardFromMainDeck,
    removeCardFromExtraDeck,
    removeCardFromSideDeck,
    getMainDeck,
    getExtraDeck,
    getSideDeck
} from './deck.js'; // Import the deck methods


// Function to handle adding a card to a deck
function handleAddToDeck(deckType, deck, card, onComplete) {
    
    const maxMainCards = 60;
    const maxExtraCards = 15;
    const maxSideCards = 15;

    switch (deckType) {
        case 'MainDeck':
            if (deck.main_deck.length >= maxMainCards) {
                window.alert(`Invalid drop: Maximum Main Deck cards allowed.`);
                return;
            }
            addCardToMainDeck(deck, card);
            break;
        case 'ExtraDeck':
            if (deck.extra_deck.length >= maxExtraCards) {
                window.alert(`Invalid drop: Maximum Extra Deck cards allowed.`);
                return;
            }
            addCardToExtraDeck(deck, card);
            break;
        case 'SideDeck':
            if (deck.side_deck.length >= maxSideCards) {
                window.alert(`Invalid drop: Maximum Side Deck cards allowed.`);
                return;
            }
            addCardToSideDeck(deck, card);
            break;
        default:
        // Handle unsupported deck type
    }
    renderDeckCards(getDeck(deck, deckType), `.DeckBuilder_Container_${deckType}`);
    updateDeckCount(deck);
    console.log(`Added "${card.name} id: ${card.id}" to the ${deckType}.`);
    // Call the onComplete function if provided
    if (typeof onComplete === 'function') {
        onComplete();
    }
}
function getDeck(deck, deckType) {
    switch (deckType) {
        case 'MainDeck':
            return deck.main_deck;
            break;
        case 'ExtraDeck':
            return deck.extra_deck;
            break;
        case 'SideDeck':
            return deck.side_deck;
            break;
        default:
    }
}
// Function to handle removing a card from a deck
function handleRemoveFromDeck(deckType, deck, card, onComplete) {
    switch (deckType) {
        case 'MainDeck':
            removeCardFromMainDeck(deck, card);
            break;
        case 'ExtraDeck':
            removeCardFromExtraDeck(deck, card);
            break;
        case 'SideDeck':
            removeCardFromSideDeck(deck, card);
            break;
        default:
        // Handle unsupported deck type
    }
    renderDeckCards(getDeck(deck, deckType), `.DeckBuilder_Container_${deckType}`);
    updateDeckCount(deck);
    console.log(`Removed card id: ${card} from the ${deckType}.`);
    // Call the onComplete function if provided
    if (typeof onComplete === 'function') {
        onComplete();
    }
}
// Function to update the displayed decks
function updateDisplayedDecks() {
    renderDeckCards(decks.main_deck, '.DeckBuilder_Container_MainDeck');
    renderDeckCards(decks.extra_deck, '.DeckBuilder_Container_ExtraDeck');
    renderDeckCards(decks.side_deck, '.DeckBuilder_Container_SideDeck');
}



// Function to update the main deck card count
function updateDeckCount(deck) {
    const mainDeckCardCountElement = document.getElementById('mainDeckCardCount');
    const extraDeckCardCountElement = document.getElementById('extraDeckCardCount');
    const sideDeckCardCountElement = document.getElementById('sideDeckCardCount');
    const mainDeckCount = deck.main_deck.length;
    const extraDeckCount = deck.extra_deck.length;
    const sideDeckCount = deck.side_deck.length;
    mainDeckCardCountElement.textContent = `#: ${mainDeckCount}`;
    extraDeckCardCountElement.textContent = `#: ${extraDeckCount}`;
    sideDeckCardCountElement.textContent = `#: ${sideDeckCount}`
}




export { handleAddToDeck, handleRemoveFromDeck, updateDisplayedDecks, updateDeckCount };
