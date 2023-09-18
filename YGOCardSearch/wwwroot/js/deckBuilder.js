import { Deck } from './deck.js';
import { renderDeckCards } from './rendering.js';
import { renderSearchedCards } from './rendering.js';
import { handleAddToDeck, handleRemoveFromDeck } from './interaction.js';

document.addEventListener('DOMContentLoaded', function () {
    // Declare deck within the event listener scope
    const deck = new Deck(deckData);
    console.log(deck.deck_name);
    console.log(deck.mainDeck.length); // Corrected the typo in 'length'
    renderCards(deck, searchedCards);


    // Function to add the dragstart listener to card elements
    function addDragStartListenerToCards() {
        // Add drag-and-drop listeners to card elements
        const cardElements = document.querySelectorAll('.inner.deckView');
        cardElements.forEach(function (cardElement) {
            cardElement.draggable = true;
            cardElement.addEventListener('dragstart', dragStart);
        });
    }

    function renderCards(deck, searchedCards) {
        renderDeck(deck);
        renderSearch(searchedCards);
        addDragStartListenerToCards();
        addDropEventListenersToTargets();
    }
    
    // Function to render the entire deck cards
    function renderDeck(deck) {
        const mainDeckContainer = '.DeckBuilder_Container_MainDeck';
        const extraDeckContainer = '.DeckBuilder_Container_ExtraDeck';
        const sideDeckContainer = '.DeckBuilder_Container_SideDeck';

        renderDeckCards(deck.getMainDeck(), mainDeckContainer); //onRenderingComplete()
        renderDeckCards(deck.getExtraDeck(), extraDeckContainer);
        renderDeckCards(deck.getSideDeck(), sideDeckContainer);
        
    }
    // Function to render the searched cards part
    function renderSearch(searchedCards) {
        renderSearchedCards(searchedCards, '.DeckBuilder_CardSearch_JS');
    }

    // Callback function to add drop event listeners after rendering is done
    function onRenderingComplete() {
        addDropEventListenersToTargets();
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
        let selectedCard = event.srcElement.dataset.cardType;
        const cardId = event.dataTransfer.getData('cardId'); // this is returning the url
        const cardType = event.dataTransfer.getData('cardType');
        const fromDeckType = event.dataTransfer.getData('fromDeckType');
        const cardIdInt = parseInt(cardId, 10);
        const cardElement = document.getElementById(cardId);

       
        // Determine the deck type based on the drop target's class and ID
        const dropTarget = event.currentTarget;
        let dropDeckType = '';
        
        // Find the card object from searchedCards based on cardId
       

        if (dropTarget.classList.contains('DeckBuilder_Container_MainDeck') && dropTarget.id === 'main-deck') {
            dropDeckType = 'MainDeck';
        } else if (dropTarget.classList.contains('DeckBuilder_Container_ExtraDeck') && dropTarget.id === 'extra-deck') {
            dropDeckType = 'ExtraDeck';
        } else if (dropTarget.classList.contains('DeckBuilder_Container_SideDeck') && dropTarget.id === 'side-deck') {
            dropDeckType = 'SideDeck';
        } else if (dropTarget.classList.contains('remove-area')) {
            dropDeckType = 'RemoveArea';
        }

        let droppedCard;
////////////////
        // Handle the case where the card is dropped outside of a deck area
        if (dropDeckType === 'RemoveArea') {
            // Remove the card from its respective deck based on 'fromDeckType'
            switch (fromDeckType) {
                case '.DeckBuilder_Container_MainDeck':
                    handleRemoveFromDeck('MainDeck', deck, cardIdInt, function () {
                        addDragStartListenerToCards();
                    });
                    break;
                case '.DeckBuilder_Container_ExtraDeck':
                    handleRemoveFromDeck('ExtraDeck', deck, cardIdInt, function() {
                        addDragStartListenerToCards();
                    });
                    break;
                case '.DeckBuilder_Container_SideDeck':
                    handleRemoveFromDeck('SideDeck', deck, cardIdInt, function() {
                        addDragStartListenerToCards();
                    });
                    break;
                default:
                    break;
            }
        } else {
            // Handle the case where the card is dropped into a valid deck area
            // Add the card to the deck based on 'deckType'
            // You need to implement your logic to add the card to the deck
            switch (fromDeckType) {
                case '.DeckBuilder_Container_MainDeck':
                    droppedCard = deck.getMainDeck().find(c=>c.id === cardIdInt);
                    break;
                case '.DeckBuilder_Container_ExtraDeck':
                    droppedCard = deck.getExtraDeck().find(c=>c.id === cardIdInt);
                    break;
                case '.DeckBuilder_Container_SideDeck':
                    droppedCard = deck.getSideDeck().find(c=>c.id === cardIdInt);
                    break;
                case '.DeckBuilder_CardSearch_JS':
                    droppedCard = searchedCards.find(c=>c.id === cardIdInt);
                    break;
                default:
                    break;
            }
    
            if (droppedCard) {
                // Add logic here for what to do when a card is dropped
                // Now you have access to the entire droppedCard object and dynamic deckType
                handleAddToDeck(dropDeckType, deck, droppedCard, function () {
                    addDragStartListenerToCards();
                });
            } else {
                // Handle the case where the card was not found
                // console.log(`Card with ID ${card.id} not found.`);
            }
        }
        //////
        
    }

    // // Add a dragover listener to the drop target (e.g., the deck)
    // const dropTarget = document.querySelector('.DeckBuilder_Container_MainDeck, .DeckBuilder_Container_ExtraDeck, .DeckBuilder_Container_SideDeck, remove-area');
    // dropTarget.addEventListener('dragover', dragOver);
    // // Add a drop listener to the drop target
    // dropTarget.addEventListener('drop', drop);

    // Function to handle the drag start event
    function dragStart(event) {
        console.log()
        let MainDeckCard = false;
        let ExtraDeckCard = false;
        const cardId = event.target.id; // Get the card ID from the dragged element's ID
        const cardIntId = parseInt(cardId, 10);
        let fromDeckType = event.target.dataset.fromDeckType; // Where this comes frome
        let draggedCard;
        const isValidForMainDeck = false;

        switch (fromDeckType) {
            case '.DeckBuilder_Container_MainDeck':
                draggedCard = deck.getMainDeck().find(c=>c.id === cardIntId);
                event.target.dataset.id = draggedCard.id;
                event.target.dataset.card = draggedCard;
                ////srcElement.dataset.cardType
                if (draggedCard) { 
                    const cardType = draggedCard.type; // Get the card type from the card object
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); // Set the card type in 'cardType'
                } else {
                    console.log(`Card with ID ${cardId} not found in Main Deck.`);
                }
                break;
            case '.DeckBuilder_Container_ExtraDeck':
                draggedCard = deck.getExtraDeck().find(c=>c.id === cardIntId);
                event.target.dataset.id = draggedCard.id;
                event.target.dataset.card = draggedCard;
                if (draggedCard) {
                    const cardType = draggedCard.type;
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); 
                }
                else {
                    console.log(`Card with ID ${cardId} not found in Extra Deck.`);
                }
                break;
            case '.DeckBuilder_Container_SideDeck':
                draggedCard = deck.getSideDeck().find(c=>c.id === cardIntId);
                event.target.dataset.id = draggedCard.id;
                if (draggedCard) {
                    const cardType = draggedCard.type;
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); 
                }
                else {
                    console.log(`Card with ID ${cardId} not found in Side Deck.`);
                }
                break;
            case '.DeckBuilder_CardSearch_JS':
                // Find the card object from searchedCards based on cardId
                draggedCard = searchedCards.find(c => c.id === cardIntId);
                event.target.dataset.card = draggedCard;
        
                if (draggedCard) { 
                    const cardType = draggedCard.type; // Get the card type from the card object
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); 
                } else {
                    console.log(`Card with ID ${cardId} not found in searchedCards.`);
                }
                break;

            default:
                console.log('Card not found');
                break;
        }
        // Set the data as attributes on the dragged element
        event.target.setAttribute('data-card-id', cardId);
        event.target.setAttribute('data-card-type', draggedCard.type);
        event.target.setAttribute('data-from-deck-type', fromDeckType);
    }
    function dragOver(event) {
        event.preventDefault();
        console.log("card:" + event.srcElement.id);
        console.log("draggin over: "+ event.target.id);
        // // Add the 'dragover' class to the container when dragging over it
        // event.currentTarget.classList.add('dragover');
    }
    // Function to handle the drag leave event
    function dragLeave(event) {
        // Remove the 'dragover' class from the container when leaving
        event.currentTarget.classList.remove('dragover');
    }
    function addDropEventListenersToTargets() {
        // Find and select the drop target divs
        const mainDeckContainer = document.querySelector('.DeckBuilder_Container_MainDeck');
        const extraDeckContainer = document.querySelector('.DeckBuilder_Container_ExtraDeck');
        const sideDeckContainer = document.querySelector('.DeckBuilder_Container_SideDeck');
        const removeAreaContainer = document.querySelector('.remove-area');

        // Add dragover event listeners to each drop target
        mainDeckContainer.addEventListener('dragover', dragOver);
        extraDeckContainer.addEventListener('dragover', dragOver);
        sideDeckContainer.addEventListener('dragover', dragOver);
        removeAreaContainer.addEventListener('dragover', dragOver);

        mainDeckContainer.addEventListener('dragleave', dragLeave);
        extraDeckContainer.addEventListener('dragleave', dragLeave);
        sideDeckContainer.addEventListener('dragleave', dragLeave);
        removeAreaContainer.addEventListener('dragleave', dragLeave);

        // Add drop event listeners to each drop target
        mainDeckContainer.addEventListener('drop', drop);
        extraDeckContainer.addEventListener('drop', drop);
        sideDeckContainer.addEventListener('drop', drop);
        removeAreaContainer.addEventListener('drop', drop);
    }

    // Call this function to add event listeners to the drop target divs
    addDropEventListenersToTargets();
    addDragStartListenerToCards();


});
