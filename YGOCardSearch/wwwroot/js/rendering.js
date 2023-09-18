// Import the renderSearchedCards function if it's in a different module
// import { renderSearchedCards } from './your-path-to-render-searched-cards.js';


function renderDeckCards(deck, deckPart) {
    var deckContainer = document.querySelector(deckPart);
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
}


// Function to render searched cards

function renderSearchedCards(cards, containerSelector) {
    var container = document.querySelector(containerSelector);
    container.innerHTML = ''; // Clear the container
   


    if (cards && cards.length > 0) {
        var cardTable = document.createElement('table');
        cardTable.className = 'searchTable table table-dark'; // Add your table classes here if needed

        var tableHeader = document.createElement('thead');
        var headerRow = document.createElement('tr');

        var headerImageCell = document.createElement('th');
        headerImageCell.textContent = 'Image';

        var headerNameCell = document.createElement('th');
        headerNameCell.textContent = 'Name';

        var headerDescriptionCell = document.createElement('th');
        headerDescriptionCell.textContent = 'Description';

        var headerTypeCell = document.createElement('th');
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
            cardImage.className = 'inner searchCardImage';
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
            var cardDescription = card.desc.length > 40 ? card.desc.substring(0, 40) + '...' : card.desc;
            cardDescriptionCell.textContent = cardDescription;

            var cardTypeCell = document.createElement('td');
            cardTypeCell.textContent = card.type;

            var cardRow = document.createElement('tr');
            cardRow.appendChild(cardImageCell);
            cardRow.appendChild(cardNameCell);
            cardRow.appendChild(cardDescriptionCell);
            cardRow.appendChild(cardTypeCell);

            var cardView = document.createElement('span');
            cardView.className = 'inner deckView';
            cardView.id = card.id;
            cardView.appendChild(cardRow);

            tableBody.appendChild(cardView);
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

