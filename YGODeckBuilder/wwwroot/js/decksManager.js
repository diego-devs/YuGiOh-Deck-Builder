function duplicateDeck(deckName) {
    fetch('/api/deck/duplicate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ deckName: deckName })
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

function renameDeck(deckName) {
    fetch('/api/deck/rename', {
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
                alert('Error renaming deck.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error renaming deck.');
        });
}

function deleteDeck(deckName) {
    fetch('/api/deck/delete', {
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