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

    // Function to handle the drop event
    function drop(event) {
        event.preventDefault();
        const cardId = event.dataTransfer.getData('text/plain');
        const cardIdInt = parseInt(cardId, 10);

        // Determine the deck type based on the drop target's class and ID
        const dropTarget = event.currentTarget;
        let deckType = '';

        if (dropTarget.classList.contains('DeckBuilder_Container_MainDeck') && dropTarget.id === 'main-deck') {
            deckType = 'MainDeck';
        } else if (dropTarget.classList.contains('DeckBuilder_Container_ExtraDeck') && dropTarget.id === 'extra-deck') {
            deckType = 'ExtraDeck';
        } else if (dropTarget.classList.contains('DeckBuilder_Container_SideDeck') && dropTarget.id === 'side-deck') {
            deckType = 'SideDeck';
        }

        const cardType = event.dataTransfer.getData('cardType');

        // Find the card object from searchedCards based on cardId
        let droppedCard = searchedCards.find(c => c.id === cardIdInt);

        if (droppedCard) {
            // Add logic here for what to do when a card is dropped
            // Now you have access to the entire droppedCard object and dynamic deckType
            handleAddToDeck(deckType, deck, droppedCard);
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
    const dropTarget = document.querySelector('.DeckBuilder_Container_MainDeck, .DeckBuilder_Container_ExtraDeck, .DeckBuilder_Container_SideDeck');
    dropTarget.addEventListener('dragover', dragOver);

    // Add a drop listener to the drop target
    dropTarget.addEventListener('drop', drop);

    // Find and select the drop target divs
    const mainDeckContainer = document.querySelector('.DeckBuilder_Container_MainDeck');
    const extraDeckContainer = document.querySelector('.DeckBuilder_Container_ExtraDeck');
    const sideDeckContainer = document.querySelector('.DeckBuilder_Container_SideDeck');

    // Add dragover event listeners to each drop target
    mainDeckContainer.addEventListener('dragover', dragOver);
    extraDeckContainer.addEventListener('dragover', dragOver);
    sideDeckContainer.addEventListener('dragover', dragOver);

    mainDeckContainer.addEventListener('dragleave', dragLeave);
    extraDeckContainer.addEventListener('dragleave', dragLeave);
    sideDeckContainer.addEventListener('dragleave', dragLeave);

    // Add drop event listeners to each drop target
    mainDeckContainer.addEventListener('drop', drop);
    extraDeckContainer.addEventListener('drop', drop);
    sideDeckContainer.addEventListener('drop', drop);

    // Function to handle the drag start event
    function dragStart(event) {
        const cardId = event.target.id; // Get the card ID from the dragged element's ID
        const cardIntId = parseInt(cardId, 10);

        // Find the card object from searchedCards based on cardId
        const draggedCard = searchedCards.find(c => c.id === cardIntId);

        if (draggedCard) {
            const cardType = draggedCard.type; // Get the card type from the card object
            event.dataTransfer.setData('text/plain', cardId);
            event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
        } else {
            console.log(`Card with ID ${cardId} not found in searchedCards.`);
        }


        //// Get the card type from the card element's data attribute
        //const cardType = event.target.getAttribute('data-card-type');

        //event.dataTransfer.setData('text/plain', event.target.id);
        //event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
    }
    function dragOver(event) {
        event.preventDefault();

        // Add the 'dragover' class to the container when dragging over it
        event.currentTarget.classList.add('dragover');
    }

    // Function to handle the drag leave event
    function dragLeave(event) {
        // Remove the 'dragover' class from the container when leaving
        event.currentTarget.classList.remove('dragover');
    }

});
