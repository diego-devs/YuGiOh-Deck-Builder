// Import the renderSearchedCards function if it's in a different module
// import { renderSearchedCards } from './your-path-to-render-searched-cards.js';


function renderDeckCards(deck, deckPart) {
    var deckContainer = document.querySelector(deckPart);
    
    if (deck.length > 0) {
        deckContainer.innerHTML = '';
        deck.forEach(function (card) {
            var cardImageSrc = 'images/small/' + card.id + '.jpg';

            var cardLink = document.createElement('a');
            cardLink.href = '/CardViewer?id=' + card.id;
            /*cardlink.id = card.id;*/

            var cardView = document.createElement('span');
            cardView.className = 'inner deckView'; // Preserve the class inner deckView
            cardView.draggable = true;
            cardView.id = card.id;

            var cardImage = document.createElement('img');
            cardImage.src = cardImageSrc;
            cardImage.className = 'inner deckCardImage'; // Apply the class inner deckCardImage to the image
            cardImage.alt = '' + card.name;
            cardImage.id = card.id;
            cardImage.dataset.cardType = card.type;
            cardImage.dataset.fromDeckType = deckPart;

            cardLink.appendChild(cardImage);
            cardView.appendChild(cardLink);

            deckContainer.appendChild(cardView);
        });
    } else if (deck.length === 0) {
        var message = 'Add cards to this deck.'
        deckContainer.textContent = message;
    }
    
}


// Function to render searched cards
// This is so messed up

function renderSearchedCards(cards, containerSelector) {
    var container = document.querySelector(containerSelector);
    container.innerHTML = ''; // Clear the container

    if (cards && cards.length > 0) {
        var cardTable = document.createElement('table');
        cardTable.className = 'searchTable table table-dark'; // Add your table classes here if needed

        var tableHeader = document.createElement('thead');
        var headerRow = document.createElement('tr');

        var headerImageCell = document.createElement('td'); // Change from th to td
        headerImageCell.textContent = 'Image';

        var headerNameCell = document.createElement('td'); // Change from th to td
        headerNameCell.textContent = 'Name';

        var headerDescriptionCell = document.createElement('td'); // Change from th to td
        headerDescriptionCell.textContent = 'Description';

        var headerTypeCell = document.createElement('td'); // Change from th to td
        headerTypeCell.textContent = 'Type';

        headerRow.appendChild(headerImageCell);
        headerRow.appendChild(headerNameCell);
        headerRow.appendChild(headerDescriptionCell);
        headerRow.appendChild(headerTypeCell);

        tableHeader.appendChild(headerRow);
        cardTable.appendChild(tableHeader);

        var tableBody = document.createElement('tbody');

        cards.forEach(function (card) {
            var cardImageSrc = 'images/small/' + card.id + '.jpg';

            var cardLink = document.createElement('a');
            cardLink.href = '/CardViewer?id=' + card.id;

            var cardImageCell = document.createElement('td');
            var cardImage = document.createElement('img');
            cardImage.src = cardImageSrc;
            cardImage.className = 'searchCardImage';
            cardImageCell.className = 'inner'
            cardImage.alt = '' + card.name;
            cardImage.id = card.id;
            cardImage.dataset.cardType = card.type;
            cardImage.dataset.fromDeckType = containerSelector;

            cardLink.appendChild(cardImage);
            cardImageCell.appendChild(cardLink);

            var cardNameCell = document.createElement('td');
            var cardNameLink = document.createElement('a');
            cardNameLink.href = '/CardViewer?id=' + card.id;
            cardNameLink.textContent = card.name;
            cardNameCell.appendChild(cardNameLink);

            var cardDescriptionCell = document.createElement('td');
            var cardDescription = card.desc.length > 100 ? card.desc.substring(0, 100) + '...' : card.desc;
            cardDescriptionCell.textContent = cardDescription;
            cardDescriptionCell.className = 'smaller-description';

            var cardTypeCell = document.createElement('td');
            cardTypeCell.textContent = card.type;

            var cardRow = document.createElement('tr');
            cardRow.appendChild(cardImageCell);
            cardRow.appendChild(cardNameCell);
            cardRow.appendChild(cardDescriptionCell);
            cardRow.appendChild(cardTypeCell);

            tableBody.appendChild(cardRow);
        });

        cardTable.appendChild(tableBody);

        // Append the table to the container
        container.appendChild(cardTable);
    } else {
        var noCardsMessage = document.createElement('p');
        noCardsMessage.textContent = 'No cards found.';
        container.appendChild(noCardsMessage);
    }
}

export { renderDeckCards, renderSearchedCards };

