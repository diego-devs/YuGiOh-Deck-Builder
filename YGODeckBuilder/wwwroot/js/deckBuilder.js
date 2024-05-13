import { Deck } from './deck.js';
import { renderDeckCards } from './rendering.js';
import { renderSearchedCards } from './rendering.js';
import { handleAddToDeck, handleRemoveFromDeck, updateDeckCount } from './interaction.js';

document.addEventListener('DOMContentLoaded', function () {
    console.log(searchedCards);
    console.log(deckData);
    // Declare deck within the event listener scope
    const deck = new Deck(deckData);
    console.log("Loaded deck: " + deck.deck_name);
    console.log(deck.main_deck); 
    renderCards(deck, searchedCards);
    updateDeckCount(deck);
    // Call this function to add event listeners to the drop target divs
    addDropEventListenersToTargets();
    addDragStartListenerToCards();
    addListenersToButtons();

    // This is for the left big card image details
    function addHoverListenerToCards() { 
        const cardElements = document.querySelectorAll('.searchCardImage, .deckCardImage');

        const bigCardImage = document.getElementById('bigCard');
        const detailText = document.getElementById('detail-text');
        const detailName = document.getElementById('detail-name');
        const detailType = document.getElementById('detail-type');
        const detailAttribute = document.getElementById('detail-attribute');
        const detailLevel = document.getElementById('detail-level');
        const detailArchetype = document.getElementById('detail-archetype');
        const detailRace = document.getElementById('detail-race');

        cardElements.forEach(cardElement => {
            cardElement.addEventListener('mouseover', function (event) {
                const cardId = event.target.id;
                console.log('Hover on card ID: ' + cardId);

                // Images should be stored in wwwroot/images/
                const cardImagePath = 'images/' + cardId + '.jpg';
                bigCardImage.src = cardImagePath;
                detailName.textContent = event.target.dataset.cardName;
                detailText.textContent = event.target.dataset.cardDescription;
                detailType.textContent = event.target.dataset.cardType;
                detailAttribute.textContent = event.target.dataset.cardAttribute ?? '';
                detailLevel.textContent = event.target.dataset.cardLevel;
                detailArchetype.textContent = event.target.dataset.cardArchetype;
                detailRace.textContent = event.target.dataset.cardRace;
                    
                
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
    // Add Drop event listener to all deck types targets
    function addDropEventListenersToTargets() {
        // Find and select the drop target divs
        const mainDeckContainer = document.querySelector('.DeckBuilder_Container_MainDeck');
        const extraDeckContainer = document.querySelector('.DeckBuilder_Container_ExtraDeck');
        const sideDeckContainer = document.querySelector('.DeckBuilder_Container_SideDeck');
/*        const removeAreaContainer = document.querySelector('.remove-area');*/

        // Add dragover event listeners to each drop target
        mainDeckContainer.addEventListener('dragover', dragOver);
        extraDeckContainer.addEventListener('dragover', dragOver);
        sideDeckContainer.addEventListener('dragover', dragOver);
        //removeAreaContainer.addEventListener('dragover', dragOver);

        mainDeckContainer.addEventListener('dragleave', dragLeave);
        extraDeckContainer.addEventListener('dragleave', dragLeave);
        sideDeckContainer.addEventListener('dragleave', dragLeave);
        //removeAreaContainer.addEventListener('dragleave', dragLeave);

        // Add drop event listeners to each drop target
        mainDeckContainer.addEventListener('drop', drop);
        extraDeckContainer.addEventListener('drop', drop);
        sideDeckContainer.addEventListener('drop', drop);
        //removeAreaContainer.addEventListener('drop', drop);
    }

    // Function to render all deck and search cards.
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

        addHoverListenerToCards();
        addDropEventListenersToTargets();
        addDragStartListenerToCards();
        addRightClickListeners(deck)
     
        
    }
    // Function to render the searched cards part
    function renderSearch(searchedCards) {
        renderSearchedCards(searchedCards, '.DeckBuilder_CardSearch_JS');
        addHoverListenerToCards();
    }

    // Handle right click
    // Function to add right-click event listeners to rendered cards
    function addRightClickListeners(deck) {
        const mainDeckContainer = '.DeckBuilder_Container_MainDeck';
        const extraDeckContainer = '.DeckBuilder_Container_ExtraDeck';
        const sideDeckContainer = '.DeckBuilder_Container_SideDeck';

        addRightClickListenersToContainer(mainDeckContainer, deck);
        addRightClickListenersToContainer(extraDeckContainer, deck);
        addRightClickListenersToContainer(sideDeckContainer, deck);
        
    }

    // Function to add right-click event listeners to cards within a specific container
    function addRightClickListenersToContainer(containerSelector, deck) {
        const container = document.querySelector(containerSelector);

        // Select all elements with the class 'deckCardImage'
        const deckCardImages = container.querySelectorAll('.deckCardImage');

        // Attach right-click event listener to each element with the class 'deckCardImage'
        deckCardImages.forEach(deckCardImage => {
            deckCardImage.addEventListener("contextmenu", function (event) {
                event.preventDefault();

                // Access information about the target element
                const cardId = parseInt(deckCardImage.id, 10);
                
                const cardType = deckCardImage.dataset.cardType;
                const fromDeckType = deckCardImage.getAttribute('data-from-deck-type');

                // Use the extracted information as needed
                console.log(`Right-clicked on card ID: ${cardId}, Type: ${cardType}, From Deck Type: ${fromDeckType}`);

                handleRightClick(fromDeckType, deck, cardId);
            });
        });
    }

    function handleRightClick(fromDeckType, deck, cardId) {
        // Remove the card from its respective deck based on 'fromDeckType'
        switch (fromDeckType) {
            case '.DeckBuilder_Container_MainDeck':
                handleRemoveFromDeck('MainDeck', deck, cardId, function () {
                    addDragStartListenerToCards();
                    addHoverListenerToCards();
                    addRightClickListeners(deck);
                });
                break;
            case '.DeckBuilder_Container_ExtraDeck':
                handleRemoveFromDeck('ExtraDeck', deck, cardId, function () {
                    addDragStartListenerToCards();
                    addHoverListenerToCards();
                    addRightClickListeners(deck);
                });
                break;
            case '.DeckBuilder_Container_SideDeck':
                handleRemoveFromDeck('SideDeck', deck, cardId, function () {
                    addDragStartListenerToCards();
                    addHoverListenerToCards();
                    addRightClickListeners(deck);
                });
                break;
            default:
                break;
        }
    }




    // Handle drag and drop: 

    // Function to handle the drop event 
    function drop(event) {
        event.preventDefault();
        let selectedCard = event.srcElement.dataset.cardType;
        const cardId = event.dataTransfer.getData('cardId'); // this is returning the url
        const cardType = event.dataTransfer.getData('cardType'); // card type: monster, spell, etc
        const fromDeckType = event.dataTransfer.getData('fromDeckType'); // Dragged from a deck
        const cardIdInt = parseInt(cardId, 10); // card konamiID as integer

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
                        addHoverListenerToCards();
                        addRightClickListeners(deck);
                    });
                    break;
                case '.DeckBuilder_Container_ExtraDeck':
                    handleRemoveFromDeck('ExtraDeck', deck, cardIdInt, function () {
                        addDragStartListenerToCards();
                        addHoverListenerToCards();
                        addRightClickListeners(deck);
                    });
                    break;
                case '.DeckBuilder_Container_SideDeck':
                    handleRemoveFromDeck('SideDeck', deck, cardIdInt, function () {
                        addDragStartListenerToCards();
                        addHoverListenerToCards();
                        addRightClickListeners(deck);
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
                                addRightClickListeners(deck);
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
                                addRightClickListeners(deck);
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
                                addRightClickListeners(deck);
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
    // Drag interaction
    function dragStart(event) {
        const cardId = event.target.id; // Get the card ID from the dragged element's ID
        const cardIntId = parseInt(cardId, 10);
        let fromDeckType = event.target.dataset.fromDeckType; // Where this comes frome
        let draggedCard;

        switch (fromDeckType) {
            case '.DeckBuilder_Container_MainDeck':
                draggedCard = deck.getMainDeck().find(c => c.id === cardIntId);
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
                draggedCard = deck.getExtraDeck().find(c => c.id === cardIntId);
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
                draggedCard = deck.getSideDeck().find(c => c.id === cardIntId);
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
        console.log("draggin over: " + event.target.id);
        // // Add the 'dragover' class to the container when dragging over it
        // event.currentTarget.classList.add('dragover');
    }
    function dragLeave(event) {
        // Remove the 'dragover' class from the container when leaving
        event.currentTarget.classList.remove('dragover');
    }

    // Define card types that are valid for the Deck Types
    const validMainDeckTypes = [
        "Normal Monster",
        "Effect Monster",
        "Flip Effect Monster",
        "Flip Tuner Effect Monster",
        "Gemini Monster",
        "Normal Tuner Monster",
        "Pendulum Effect Monster",
        "Pendulum Effect Ritual Monster",
        "Pendulum Flip Effect Monster",
        "Pendulum Normal Monster",
        "Pendulum Tuner Effect Monster",
        "Toon Monster",
        "Spirit Monster",
        "Union Effect Monster",
        "Tuner Monster",
        "Ritual Effect Monster",
        "Ritual Monster",
        "Spell Card",
        "Trap Card"
    ];
    const validExtraDeckTypes = [
        "Fusion Monster",
        "Pendulum Effect Fusion Monster",
        "Synchro Monster",
        "Synchro Pendulum Effect Monster",
        "Synchro Tuner Monster",
        "XYZ Monster",
        "XYZ Pendulum Effect Monster",
        "Link Monster",
    ];
    // For ruling: valid main and extra card types
    function isCardValidForMainDeck(cardType) {
        
        if (validMainDeckTypes.includes(cardType)) {
            return true;
        } else if (validExtraDeckTypes.includes(cardType)) {
            return false;
        }

        // Check if the card type is in the list of valid Main Deck types
        //return validMainDeckTypes.includes(cardType);
    }
    // For ruling: no more than 3 same cards per deck
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
    // Saving deck. Making call to C# server
    
    // AJAX Call to our C# API to save the deck correctly 
    function saveDeckToApi() {
        // Get the current deck data
        //const mainDeckData = JSON.stringify(deck.getMainDeck());
        //const extraDeckData = JSON.stringify(deck.getExtraDeck());
        //const sideDeckData = JSON.stringify(deck.getSideDeck());

        // Create an object to hold the deck data
        const deckData = {
            deck_name: deck.deck_name,
            main_deck: deck.getMainDeck(),
            extra_deck: deck.getExtraDeck(),
            side_deck: deck.getSideDeck(),
        };

        // Send the deck data to the server
        fetch('/api/deck/save', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(deck),
        })
            .then(response => {
                if (response.ok) {
                    // Handle success (e.g., show a success message)
                    console.log('Deck saved successfully');
                    // Reload the page
                    location.reload();
                } else {
                    // Handle errors (e.g., show an error message)
                    console.error('Failed response from the server. The deck was not saved.');
                }
            })
            .catch(error => {
                // Handle network errors
                console.error('Network error:', error);
            });
    }
    function addListenersToButtons() {
        // Save the deck making a call to Api
        document.getElementById('saveDeckButton').addEventListener('click', function () {
            saveDeckToApi();
        });
        // Remove all cards from deck
        document.getElementById('clearDeckButton').addEventListener('click', function () {
            clearDeck();
        });
        // Sorts deck
        document.getElementById('sortDeckButton').addEventListener('click', function () {
            sortCards();
        });

        document.getElementById('shuffleDeckButton').addEventListener('click', function () {
            shuffleDeck();
        });
    }
    function sortCards() {
        // Sort the Main Deck
        sortDeck(deck.getMainDeck(), validMainDeckTypes);
        // Sort the Extra Deck
        sortDeck(deck.getExtraDeck(), validExtraDeckTypes);
        // Sort the Side Deck
        sortDeck(deck.getSideDeck(), validMainDeckTypes);
        renderDeck(deck);
    }
    // Function to sort a deck by type and name
    // Function to sort a deck by type, level, and name
    function sortDeck(deck, validTypes) {
        deck.sort((a, b) => {
            // Get the index of the card types in the ordering array
            const typeIndexA = validTypes.indexOf(a.type);
            const typeIndexB = validTypes.indexOf(b.type);

            // Compare card types based on the custom ordering
            const typeComparison = typeIndexA - typeIndexB;
            if (typeComparison !== 0) {
                return typeComparison;
            }

            // If card types are the same, sort by level (from highest to lowest)
            const levelComparison = b.level - a.level;
            if (levelComparison !== 0) {
                return levelComparison;
            }

            // If levels are the same, sort by name
            const nameA = a.name || "";
            const nameB = b.name || "";
            return nameA.localeCompare(nameB);
        });
    }
    function clearDeck() {
        deck.main_deck = [];
        deck.extra_deck = [];
        deck.side_deck = [];
        // Re-render the sorted deck in your UI
        renderDeck(deck);
        // You can add additional logic to update the UI here if needed
    }
    function shuffleDeck() {
        deck.main_deck = shuffleArray(deck.main_deck);
        deck.extra_deck = shuffleArray(deck.extra_deck);
        deck.side_deck = shuffleArray(deck.side_deck);
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
