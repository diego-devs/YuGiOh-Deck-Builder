class Deck {
    constructor(deckData) {
        this.deck_id = deckData.deck_id;
        this.deck_name = deckData.deck_name;
        this.mainDeck = deckData.mainDeck;
        this.extraDeck = deckData.extraDeck;
        this.sideDeck = deckData.sideDeck;
        this.totalCards = deckData.TotalCards;
    }

    // Implement methods to add, remove, and get cards from the decks
    addCardToMainDeck(card) {
        this.mainDeck.push(card);
    }

    addCardToExtraDeck(card) {
        this.extraDeck.push(card);
    }

    addCardToSideDeck(card) {
        this.sideDeck.push(card);
    }

    removeCardFromMainDeck(cardId) {
        this.mainDeck = this.mainDeck.filter(card => card.id !== cardId);
    }

    removeCardFromExtraDeck(cardId) {
        this.extraDeck = this.extraDeck.filter(card => card.id !== cardId);
    }

    removeCardFromSideDeck(cardId) {
        this.sideDeck = this.sideDeck.filter(card => card.id !== cardId);
    }

    getMainDeck() {
        return this.mainDeck;
    }

    getExtraDeck() {
        return this.extraDeck;
    }

    getSideDeck() {
        return this.sideDeck;
    }
}
const addCardToMainDeck = (deck, card) => deck.addCardToMainDeck(card);
const addCardToExtraDeck = (deck, card) => deck.addCardToExtraDeck(card);
const addCardToSideDeck = (deck, card) => deck.addCardToSideDeck(card);
const removeCardFromMainDeck = (deck, card) => deck.removeCardFromMainDeck(card);
const removeCardFromExtraDeck = (deck, card) => deck.removeCardFromExtraDeck(card);
const removeCardFromSideDeck = (deck, card) => deck.removeCardFromSideDeck(card);
const getMainDeck = deck => deck.getMainDeck();
const getExtraDeck = deck => deck.getExtraDeck();
const getSideDeck = deck => deck.getSideDeck();

export {
    Deck,
    addCardToMainDeck,
    addCardToExtraDeck,
    addCardToSideDeck,
    removeCardFromMainDeck,
    removeCardFromExtraDeck,
    removeCardFromSideDeck,
    getMainDeck,
    getExtraDeck,
    getSideDeck
};
