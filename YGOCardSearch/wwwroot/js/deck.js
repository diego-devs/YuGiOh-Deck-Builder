class Deck {
    constructor(deckData) {
        this.deck_id = deckData.deck_id;
        this.deck_name = deckData.deck_name;
        this.main_deck = deckData.main_deck;
        this.extra_deck = deckData.extra_deck;
        this.side_deck = deckData.side_deck;
        this.total_cards = deckData.total_cards;
    }

    // Implement methods to add, remove, and get cards from the decks
    addCardToMainDeck(card) {
        this.main_deck.push(card);
    }

    addCardToExtraDeck(card) {
        this.extra_deck.push(card);
    }

    addCardToSideDeck(card) {
        this.side_deck.push(card);
    }

    removeCardFromMainDeck(cardId) {
        const index = this.main_deck.findIndex(card => card.id === cardId);
        if (index !== -1) {
            this.main_deck.splice(index, 1);
        }
    }

    removeCardFromExtraDeck(cardId) {
        const index = this.extra_deck.findIndex(card => card.id === cardId);
        if (index !== -1) {
            this.extra_deck.splice(index, 1);
        }
    }

    removeCardFromSideDeck(cardId) {
        const index = this.side_deck.findIndex(card => card.id === cardId);
        if (index !== -1) {
            this.side_deck.splice(index, 1);
        }
    }

    getDeckName() {
        return this.deck_name;
    }

    getMainDeck() {
        return this.main_deck;
    }

    getExtraDeck() {
        return this.extra_deck;
    }

    getSideDeck() {
        return this.side_deck;
    }

    getCardCount() {
        return this.main_deck.length + this.extra_deck.length + this.side_deck.length;
    }

    // Not used
    getStrongestCard() {
        let strongestCard = null;
        let highestAttackPoints = -1; 
        // Check cards in the main deck
        this.main_deck.forEach(card => {
            if (card.atk >= 0 && card.atk > highestAttackPoints) {
                strongestCard = card;
                highestAttackPoints = card.atk;
            }
        });
    
        // Check cards in the extra deck
        this.extra_deck.forEach(card => {
            if (card.atk >= 0 && card.atk > highestAttackPoints) {
                strongestCard = card;
                highestAttackPoints = card.atk;
            }
        });
    
        return strongestCard;
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
