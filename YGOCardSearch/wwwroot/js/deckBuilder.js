import { Deck } from './deck.js';
import { renderDeckCards } from './rendering.js';
import { renderSearchedCards } from './rendering.js';
import { handleAddToDeck, handleRemoveFromDeck, updateDeckCount } from './interaction.js';

document.addEventListener('DOMContentLoaded', function () {
    // Declare deck within the event listener scope
    const deck = new Deck(deckData);
    console.log(deck.deck_name);
    console.log(deck.mainDeck.length); // Corrected the typo in 'length'
    renderCards(deck, searchedCards);
    updateDeckCount(deck);

    
    function addHoverListenerToCards() {
        const cardElements = document.querySelectorAll('.searchCardImage, .deckCardImage');

        const bigCardImage = document.getElementById('bigCard');
        const detailText = document.getElementById('detail-text');

        cardElements.forEach(cardElement => {
            cardElement.addEventListener('mouseover', function (event) {
                const cardId = event.target.id;
                console.log('Hover on card ID: ' + cardId);

                // Assuming your big image path follows the same pattern
                const cardImagePath = 'images/' + cardId + '.jpg';

                bigCardImage.src = cardImagePath;
            });

            cardElement.addEventListener('mouseout', function () {
                /*bigCardImage.src = 'images/2511.jpg'; // Clear image when not hovering*/
            });
        });
    }



    // Function to add the dragstart listener to card elements
    function addDragStartListenerToCards() {
        // Add drag-and-drop listeners to card elements
        const cardElements = document.querySelectorAll('.inner, .deckView');
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
        addHoverListenerToCards();
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
    



    ////// Handle drag and drop functionality events: 

    // Function to handle the drop event
    function drop(event) {
        event.preventDefault();
        let selectedCard = event.srcElement.dataset.cardType;
        const cardId = event.dataTransfer.getData('cardId'); // this is returning the url
        const cardType = event.dataTransfer.getData('cardType'); // card type: monster, spell, etc
        const fromDeckType = event.dataTransfer.getData('fromDeckType'); // Dragged from a deck
        const cardIdInt = parseInt(cardId, 10); // card konamiID as integer
        const cardElement = document.getElementById(cardId);
        const isValidForMainDeck = event.dataTransfer.getData('isValidForMainDeck');// Is for Main/Side or Extra deck?

       
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
                    handleRemoveFromDeck('ExtraDeck', deck, cardIdInt, function () {
                        addDragStartListenerToCards();
                    });
                    break;
                case '.DeckBuilder_Container_SideDeck':
                    handleRemoveFromDeck('SideDeck', deck, cardIdInt, function () {
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
                    droppedCard = deck.getMainDeck().find(c => c.id === cardIdInt);
                    break;
                case '.DeckBuilder_Container_ExtraDeck':
                    droppedCard = deck.getExtraDeck().find(c => c.id === cardIdInt);
                    break;
                case '.DeckBuilder_Container_SideDeck':
                    droppedCard = deck.getSideDeck().find(c => c.id === cardIdInt);
                    break;
                case '.DeckBuilder_CardSearch_JS':
                    droppedCard = searchedCards.find(c => c.id === cardIdInt);
                    break;
                default:
                    break;
            }

            if (droppedCard) {
                // Maximum allowed copies of the card // developer todo: add banlist support
                const cardCopies = getCardCopiesInDeck(droppedCard.id);
                const maxAllowedCopies = 3; 
                if (cardCopies >= maxAllowedCopies) {
                    window.alert(`Invalid drop: You can't add more than ${maxAllowedCopies} copies of "${droppedCard.name}" to the ${dropDeckType}.`);
                    return;

                }

                switch (dropDeckType) {
                    case 'MainDeck':
                        if (isValidForMainDeck === "true") {
                            handleAddToDeck(dropDeckType, deck, droppedCard, function () {
                                addDragStartListenerToCards();
                                addHoverListenerToCards();
                            });
                        } else {
                            // Display an error message or prevent the card from being added to Main Deck
                            window.alert('Invalid drop: Card Type: ' + cardType + ' cannot be added to Main Deck.');
                        }
                        break;
                    case 'ExtraDeck':
                        if (isValidForMainDeck === "false") {
                            handleAddToDeck(dropDeckType, deck, droppedCard, function () {
                                addDragStartListenerToCards();
                                addHoverListenerToCards();
                            });          
                        } else {
                            // Display an error message or prevent the card from being added to Extra Deck
                            window.alert('Invalid drop: Card Type: ' + cardType + ' cannot be added to Extra Deck.');
                        }
                        break;
                    case 'SideDeck':
                        if (isValidForMainDeck === "true") {
                            handleAddToDeck(dropDeckType, deck, droppedCard, function () {
                                addDragStartListenerToCards();
                                addHoverListenerToCards();
                            });
                        } 
                        else {
                            // Display an error message or prevent the card from being added to Side Deck
                            window.alert('Invalid drop: Card Type: ' + cardType + ' cannot be added to Side Deck.');
                        }
                        break;
                    default:
                        break;
                }

                
            } else {
                // Handle the case where the card was not found
                console.log(`Card with ID ${cardId} not found.`);
            }
        }
        
    }

    // Define card types that are valid for the Main Deck
    const validMainDeckTypes = [
        "Effect Monster",
        "Flip Effect Monster",
        "Flip Tuner Effect Monster",
        "Gemini Monster",
        "Normal Monster",
        "Normal Tuner Monster",
        "Pendulum Effect Monster",
        "Pendulum Effect Ritual Monster",
        "Pendulum Flip Effect Monster",
        "Pendulum Normal Monster",
        "Pendulum Tuner Effect Monster",
        "Ritual Effect Monster",
        "Ritual Monster",
        "Spell Card",
        "Spirit Monster",
        "Toon Monster",
        "Trap Card",
        "Tuner Monster",
        "Union Effect Monster"
    ];

    const validExtraDeckTypes = [
        "Fusion Monster",
        "Link Monster",
        "Pendulum Effect Fusion Monster",
        "Synchro Monster",
        "Synchro Pendulum Effect Monster",
        "Synchro Tuner Monster",
        "XYZ Monster",
        "XYZ Pendulum Effect Monster"
    ];

    function isCardValidForMainDeck(cardType) {
        
        if (validMainDeckTypes.includes(cardType)) {
            return true;
        } else if (validExtraDeckTypes.includes(cardType)) {
            return false;
        }

        // Check if the card type is in the list of valid Main Deck types
        //return validMainDeckTypes.includes(cardType);
    }
    function getCardCopiesInDeck(cardId) {
        // Use the filter method to create an array of cards with the specified ID
        const mainDeckMatches = deck.getMainDeck().filter(card => card.id === cardId);
        const extraDeckMatches = deck.getExtraDeck().filter(card => card.id === cardId);
        const sideDeckMatches = deck.getSideDeck().filter(card => card.id === cardId);

        // Calculate the total count across all decks
        const totalCount = mainDeckMatches.length + extraDeckMatches.length + sideDeckMatches.length;

        // Return the total count
        return totalCount;
    }

    // drag interaction
    function dragStart(event) {
        const cardId = event.target.id; // Get the card ID from the dragged element's ID
        const cardIntId = parseInt(cardId, 10);
        let fromDeckType = event.target.dataset.fromDeckType; // Where this comes frome
        let draggedCard;

        switch (fromDeckType) {
            case '.DeckBuilder_Container_MainDeck':
                draggedCard = deck.getMainDeck().find(c=>c.id === cardIntId);
                event.target.dataset.id = draggedCard.id;
                event.target.dataset.card = draggedCard;

                ////srcElement.dataset.cardType
                if (draggedCard) { 
                    const cardType = draggedCard.type; // Get the card type from the card object
                    const isValidForMainDeck = isCardValidForMainDeck(cardType); // Determine whether the card is valid for Main Deck
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('isValidForMainDeck', isValidForMainDeck);

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
                    const isValidForMainDeck = isCardValidForMainDeck(cardType);
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); 
                    event.dataTransfer.setData('isValidForMainDeck', isValidForMainDeck);
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
                    const isValidForMainDeck = isCardValidForMainDeck(cardType);
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); 
                    event.dataTransfer.setData('isValidForMainDeck', isValidForMainDeck);
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
                    const isValidForMainDeck = isCardValidForMainDeck(cardType);
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardType', cardType); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('cardId', cardId); // Set the card type in 'cardType'
                    event.dataTransfer.setData('text/plain', cardId);
                    event.dataTransfer.setData('fromDeckType', fromDeckType); 
                    event.dataTransfer.setData('isValidForMainDeck', isValidForMainDeck);
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

    document.getElementById('saveDeckButton').addEventListener('click', function () {
        saveDeckToApi();
    });
    function saveDeckToApi() {
        // Get the current deck data
        //const mainDeckData = JSON.stringify(deck.getMainDeck());
        //const extraDeckData = JSON.stringify(deck.getExtraDeck());
        //const sideDeckData = JSON.stringify(deck.getSideDeck());

        // Create an object to hold the deck data
        const deckData = {
            deck_name: deck.deck_name,
            mainDeck: deck.getMainDeck(),
            extraDeck: deck.getExtraDeck(),
            sideDeck: deck.getSideDeck(),
        };

        // Send the deck data to the server
        fetch('/api/deck/save', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(deckData),
        })
            .then(response => {
                if (response.ok) {
                    // Handle success (e.g., show a success message)
                    console.log('Deck saved successfully');
                    // Reload the page
                    location.reload();
                } else {
                    // Handle errors (e.g., show an error message)
                    console.error('Failed to save the deck');
                }
            })
            .catch(error => {
                // Handle network errors
                console.error('Network error:', error);
            });
    }

    // Call this function to add event listeners to the drop target divs
    addDropEventListenersToTargets();
    addDragStartListenerToCards();

    document.getElementById('clearDeckButton').addEventListener('click', function () {
        clearDeck();
    });

    document.getElementById('sortDeckButton').addEventListener('click', function () {
        sortDeckByNameAndType();
    });

    document.getElementById('shuffleDeckButton').addEventListener('click', function () {
        shuffleDeck();
    });

    function sortDeckByNameAndType() {
        // Get the deck cards and sort them by name first, then by card type
        deck.getMainDeck().sort((a, b) => {
            // Get the names for comparison
            const nameA = a.name || "";
            const nameB = b.name || "";

            // If the names are different, sort by name
            if (nameA !== nameB) {
                return nameA.localeCompare(nameB);
            }

            // If the names are the same, sort by card type
            const typeA = a.type || "";
            const typeB = b.type || "";

            return typeA.localeCompare(typeB);
        });

        // Re-render the sorted deck in your UI
        renderDeck(deck);

        // You can add additional logic to update the UI here if needed
    }


    function sortDeckByCardType() {
        // Get the deck cards and sort them by card type alphabetically
        deck.getMainDeck().sort((a, b) => {
            // Get the card types for comparison
            const typeA = a.type || "";
            const typeB = b.type || "";

            // Use localeCompare to sort in alphabetical order
            return typeA.localeCompare(typeB);
        });

        // Re-render the sorted deck in your UI
        renderDeck(deck);

        // You can add additional logic to update the UI here if needed
    }
    function clearDeck() {
        deck.mainDeck = [];
        deck.extraDeck = [];
        deck.sideDeck = [];
        // Re-render the sorted deck in your UI
        renderDeck(deck);
        // You can add additional logic to update the UI here if needed
    }

    function shuffleDeck() {
        deck.mainDeck = shuffleArray(deck.mainDeck);
        deck.extraDeck = shuffleArray(deck.extraDeck);
        // Re-render the shuffled deck in your UI
        renderDeck(deck);
    }
    // Function to shuffle an array (Fisher-Yates shuffle algorithm)
    function shuffleArray(array) {
        for (let i = array.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [array[i], array[j]] = [array[j], array[i]];
        }
        return array;
    }





    
        



    

    


});
