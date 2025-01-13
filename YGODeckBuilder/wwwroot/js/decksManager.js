function duplicateDeck(deckName) {
    fetch('/api/DecksManager/duplicate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(deckName)
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
            console.error('Error:', error);
            // Handle network errors
            console.error('Network error:', error);
            alert('Error duplicating deck.');
        });
}

function showRenameInput(deckName) {
    const newDeckName = prompt("Enter new deck name:", deckName);
    if (newDeckName !== null) { // Check if the user clicked Cancel
        renameDeck(deckName, newDeckName);
    }
}

function renameDeck(oldDeckName, newDeckName) {
    const requestData = { oldDeckName: oldDeckName, newDeckName: newDeckName };
    fetch('/api/DecksManager/rename', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestData)
    })
        .then(response => {
            if (response.ok) {
                location.reload(); // Refresh the page on success
            } else {
                alert('Error renaming deck.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error renaming deck.');
        });
}
function deleteDeck(deckName) {
    fetch('/api/DecksManager/delete', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(deckName)
    })
        .then(response => {
            if (response.ok) {
                // Handle success, e.g., refresh the page or show a success message
                location.reload(); // Example: Refresh the page
            } else {
                // Handle error, e.g., show an error message
                alert('Error deleting deck.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error deleting deck.');
        });
}
function showNewDeckInput(deckName) {
    const newDeckName = prompt("Enter new deck name:", deckName);
    if (newDeckName !== null) { // Check if the user clicked Cancel
        newDeck(newDeckName);
    }
}
function newDeck(newDeckName) {
    fetch('/api/DecksManager/new', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newDeckName)
    })
        .then(response => {
            if (response.ok) {
                // Handle success, e.g., refresh the page or show a success message
                location.reload(); // Example: Refresh the page
            } else {
                // Handle error, e.g., show an error message
                alert('Error creating new deck.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error creating new deck.');
        });
}
