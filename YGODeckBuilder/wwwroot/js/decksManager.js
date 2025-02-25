function duplicateDeck(deckName) {
    fetch('/api/decks/duplicate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(deckName)
    })
        .then(response => {
            if (response.ok) {
                console.log('Deck saved successfully');
                location.reload();
            } else {
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
function showLoadInput() {
    // Simulate clicking the hidden file input
    document.getElementById('deckFileInput').click();
}
function loadDeck(event) {
    const file = event.target.files[0];
    if (!file) {
        alert('No file selected.');
        return;
    }
    if (!file.name.endsWith('.ydk')) {
        alert('Invalid file type. Please select a .ydk file.');
        return;
    }

    const reader = new FileReader();
    reader.onload = function (e) {
        const fileContent = e.target.result; // File content
        const fileName = file.name; // File name (e.g., "myDeck.ydk")

        fetch('/api/decks/load', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                DeckName: fileName,
                DeckContent: fileContent,
            }),
        })
            .then(response => {
                if (response.ok) {
                    alert('Deck loaded successfully!');
                    location.reload();
                } else {
                    alert('Failed to load the deck.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('An error occurred while loading the deck.');
            });
    };

    reader.readAsText(file);
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
    fetch('/api/decks/delete', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(deckName)
    })
        .then(response => {
            if (response.ok) {
                location.reload(); 
            } else {
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
    console.log('test');
    fetch('/api/decks/new', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newDeckName)
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                alert('Error creating new deck.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error creating new deck.');
        });
}
