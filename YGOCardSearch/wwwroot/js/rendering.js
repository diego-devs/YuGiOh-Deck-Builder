function renderDeckCards(deck, deckPart) {
    var deckContainer = document.querySelector(deckPart);
    deckContainer.innerHTML = '';

    deck.forEach(function (card) {
        var cardImageSrc = 'images/small/' + card.id + '.jpg';

        var cardLink = document.createElement('a');
        cardLink.href = '/CardViewer?id=' + card.id;

        var cardImage = document.createElement('img');
        cardImage.src = cardImageSrc;
        cardImage.className = 'inner deckCardImage';
        cardImage.alt = '' + card.Name;

        cardLink.appendChild(cardImage);

        var cardView = document.createElement('span');
        cardView.className = 'inner deckView';
        cardView.appendChild(cardLink);

        deckContainer.appendChild(cardView);
    });
}

export { renderDeckCards };
