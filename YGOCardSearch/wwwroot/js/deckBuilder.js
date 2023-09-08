import { Deck } from './deck.js';
import { renderDeckCards } from './rendering.js';
import { renderSearchedCards } from './rendering.js';
import { handleAddToDeck } from './interaction.js';

document.addEventListener('DOMContentLoaded', function () {
    // Declare deck within the event listener scope
    const deck = new Deck(deckData);
    console.log(deck.deck_name);
    console.log(deck.mainDeck.length); // Corrected the typo in 'length'
    renderDeck(deck);
    renderSearch(searchedCards);

    // Function to render the entire deck
    function renderDeck(deck) {
        const mainDeckContainer = '.DeckBuilder_Container_MainDeck';
        const extraDeckContainer = '.DeckBuilder_Container_ExtraDeck';
        const sideDeckContainer = '.DeckBuilder_Container_SideDeck';

        renderDeckCards(deck.getMainDeck(), mainDeckContainer);
        renderDeckCards(deck.getExtraDeck(), extraDeckContainer);
        renderDeckCards(deck.getSideDeck(), sideDeckContainer);
    }

    function renderSearch(searchedCards) {
        renderSearchedCards(searchedCards, '.DeckBuilder_CardSearch_JS');
    }

    // Testing add card to deck via button
    const addCardButton = document.getElementById('addCardButton');
    addCardButton.addEventListener('click', function () {
        const testC = testCard;
        handleAddToDeck('MainDeck', deck, testC);
    });

    ////// Handle drag and drop functionality events: 

    // Function to handle the drag over event
    function dragOver(event) {
        event.preventDefault();
    }

    // Function to handle the drop event
    function drop(event) {
        event.preventDefault();
        const cardId = event.dataTransfer.getData('text/plain');
        const cardIdInt = parseInt(cardId, 10);

        // Find the card object from searchedCards based on cardId
        let droppedCard = searchedCards.find(c => c.id === cardIdInt);

        if (droppedCard) {
            // Add logic here for what to do when a card is dropped
            // Now you have access to the entire droppedCard object
            handleAddToDeck('MainDeck', deck, droppedCard);
        } else {
            // Handle the case where the card was not found
            console.log(`Card with ID ${cardId} not found.`);
        }
    }


    // Add drag-and-drop listeners to card elements
    const cardElements = document.querySelectorAll('.inner.deckView');
    cardElements.forEach(function (cardElement) {
        cardElement.draggable = true;
        cardElement.addEventListener('dragstart', dragStart);
    });

    // Add a dragover listener to the drop target (e.g., the deck)
    const dropTarget = document.querySelector('.DeckBuilder_Container_MainDeck');
    dropTarget.addEventListener('dragover', dragOver);

    // Add a drop listener to the drop target
    dropTarget.addEventListener('drop', drop);

    // Function to handle the drag start event
    function dragStart(event) {
        event.dataTransfer.setData('text/plain', event.target.id);
        console.log("drag started for card:" + event.target.id);
    }
});
