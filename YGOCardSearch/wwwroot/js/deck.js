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
        const index = this.mainDeck.findIndex(card => card.id === cardId);
        if (index !== -1) {
            this.mainDeck.splice(index, 1);
        }
    }

    removeCardFromExtraDeck(cardId) {
        const index = this.extraDeck.findIndex(card => card.id === cardId);
        if (index !== -1) {
            this.extraDeck.splice(index, 1);
        }
    }

    removeCardFromSideDeck(cardId) {
        const index = this.sideDeck.findIndex(card => card.id === cardId);
        if (index !== -1) {
            this.sideDeck.splice(index, 1);
        }
    }

    getDeckName() {
        return this.deck_name;
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

    getCardCount() {
        return this.mainDeck.length + this.extraDeck.length + this.sideDeck.length;
    }

    // Not used
    getStrongestCard() {
        let strongestCard = null;
        let highestAttackPoints = -1; 
        // Check cards in the main deck
        this.mainDeck.forEach(card => {
            if (card.atk >= 0 && card.atk > highestAttackPoints) {
                strongestCard = card;
                highestAttackPoints = card.atk;
            }
        });
    
        // Check cards in the extra deck
        this.extraDeck.forEach(card => {
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
