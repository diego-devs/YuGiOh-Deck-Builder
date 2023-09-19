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
    const maxAllowedCopies = 3; // Maximum allowed copies of the same card // developer todo: add banlist support
    const cardCountInDeck = deck.mainDeck.filter((c) => c.id === card.id).length;

    if (cardCountInDeck >= maxAllowedCopies) {
        window.alert(`Invalid drop: You can't add more than ${maxAllowedCopies} copies of "${card.name}" to the ${deckType}.`);
        return;
    }

    switch (deckType) {
        case 'MainDeck':
            addCardToMainDeck(deck, card);
            break;
        case 'ExtraDeck':
            addCardToExtraDeck(deck, card);
            break;
        case 'SideDeck':
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
            return deck.mainDeck;
            break;
        case 'ExtraDeck':
            return deck.extraDeck;
            break;
        case 'SideDeck':
            return deck.sideDeck;
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
    renderDeckCards(decks.mainDeck, '.DeckBuilder_Container_MainDeck');
    renderDeckCards(decks.extraDeck, '.DeckBuilder_Container_ExtraDeck');
    renderDeckCards(decks.sideDeck, '.DeckBuilder_Container_SideDeck');
}

// Function to update the main deck card count
function updateDeckCount(deck) {
    const mainDeckCardCountElement = document.getElementById('mainDeckCardCount');
    const extraDeckCardCountElement = document.getElementById('extraDeckCardCount');
    const sideDeckCardCountElement = document.getElementById('sideDeckCardCount');
    const mainDeckCount = deck.mainDeck.length;
    const extraDeckCount = deck.extraDeck.length;
    const sideDeckCount = deck.sideDeck.length;
    mainDeckCardCountElement.textContent = `Cards: ${mainDeckCount}`;
    extraDeckCardCountElement.textContent = `Cards: ${extraDeckCount}`;
    sideDeckCardCountElement.textContent = `Cards: ${sideDeckCount}`
}



export { handleAddToDeck, handleRemoveFromDeck, updateDisplayedDecks, updateDeckCount };
