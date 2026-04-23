// ---- DOM helpers ----
function createDeckItemHtml(deckName) {
    var escaped = deckName.replace(/'/g, "\\'");
    var encoded = encodeURIComponent(deckName);
    return '<div class="list-group-item d-flex justify-content-between align-items-center bg-dark border-light" data-deck-name="' + deckName + '">' +
        '<a href="/DeckBuilder?DeckFileName=' + encoded + '" class="text-white text-uppercase">' + deckName + '</a>' +
        '<div class="btn-group" role="group" aria-label="Deck Actions">' +
        '<a class="btn btn-primary mr-1" href="/DeckBuilder?DeckFileName=' + encoded + '">Edit</a>' +
        '<button type="button" class="btn btn-success mr-1" onclick="duplicateDeck(\'' + escaped + '\')">Duplicate</button>' +
        '<button type="button" class="btn btn-warning mr-1" onclick="showRenameModal(\'' + escaped + '\')">Rename</button>' +
        '<button type="button" class="btn btn-danger" onclick="showDeleteConfirm(\'' + escaped + '\')">Delete</button>' +
        '</div></div>';
}

function getDeckList() { return document.getElementById('deckList'); }
function getEmptyMsg() { return document.getElementById('emptyDeckMessage'); }

function syncEmptyState() {
    var list = getDeckList();
    var empty = getEmptyMsg();
    if (!list || !empty) return;
    var hasItems = list.querySelectorAll('[data-deck-name]').length > 0;
    list.style.display = hasItems ? '' : 'none';
    empty.style.display = hasItems ? 'none' : '';
}

// ---- Deck actions ----
function duplicateDeck(deckName) {
    fetch('/api/decks/duplicate', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(deckName)
    })
        .then(function (response) {
            if (response.ok) {
                var copyName = 'Copy_' + deckName;
                var list = getDeckList();
                if (list) {
                    list.insertAdjacentHTML('beforeend', createDeckItemHtml(copyName));
                }
                syncEmptyState();
                showToast('Deck duplicated successfully.', 'success');
            } else {
                showToast('Failed to duplicate deck.', 'error');
            }
        })
        .catch(function (error) {
            showToast('Error duplicating deck.', 'error');
        });
}

function showLoadInput() {
    document.getElementById('deckFileInput').click();
}

function loadDeck(event) {
    var file = event.target.files[0];
    if (!file) {
        showToast('No file selected.', 'error');
        return;
    }
    if (!file.name.endsWith('.ydk')) {
        showToast('Invalid file type. Please select a .ydk file.', 'error');
        return;
    }

    var reader = new FileReader();
    reader.onload = function (e) {
        var fileContent = e.target.result;
        var fileName = file.name;

        fetch('/api/decks/load', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ DeckName: fileName, DeckContent: fileContent }),
        })
            .then(function (response) {
                if (response.ok) {
                    showToast('Deck loaded successfully!', 'success');
                    location.reload();
                } else {
                    showToast('Failed to load the deck.', 'error');
                }
            })
            .catch(function (error) {
                showToast('An error occurred while loading the deck.', 'error');
            });
    };
    reader.readAsText(file);
}

// ---- Rename (modal-based) ----
var _renamingDeckName = null;

function showRenameModal(deckName) {
    _renamingDeckName = deckName;
    var input = document.getElementById('renameInput');
    if (input) input.value = deckName;
    $('#renameModal').modal('show');
}

document.addEventListener('DOMContentLoaded', function () {
    var confirmRenameBtn = document.getElementById('confirmRenameBtn');
    if (confirmRenameBtn) {
        confirmRenameBtn.addEventListener('click', function () {
            var newName = document.getElementById('renameInput').value.trim();
            if (newName && _renamingDeckName) {
                $('#renameModal').modal('hide');
                renameDeck(_renamingDeckName, newName);
            }
        });
    }

    var confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    if (confirmDeleteBtn) {
        confirmDeleteBtn.addEventListener('click', function () {
            $('#deleteModal').modal('hide');
            if (_deletingDeckName) {
                deleteDeckConfirmed(_deletingDeckName);
            }
        });
    }
});

function renameDeck(oldDeckName, newDeckName) {
    var requestData = { oldDeckName: oldDeckName, newDeckName: newDeckName };
    fetch('/api/decks/rename', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData)
    })
        .then(function (response) {
            if (response.ok) {
                var item = document.querySelector('[data-deck-name="' + oldDeckName + '"]');
                if (item) {
                    item.outerHTML = createDeckItemHtml(newDeckName);
                }
                showToast('Deck renamed successfully.', 'success');
            } else {
                showToast('Error renaming deck.', 'error');
            }
        })
        .catch(function (error) {
            showToast('Error renaming deck.', 'error');
        });
}

// ---- Delete (modal-based) ----
var _deletingDeckName = null;

function showDeleteConfirm(deckName) {
    _deletingDeckName = deckName;
    var display = document.getElementById('deleteDeckNameDisplay');
    if (display) display.textContent = deckName;
    $('#deleteModal').modal('show');
}

function deleteDeckConfirmed(deckName) {
    fetch('/api/decks/delete', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(deckName)
    })
        .then(function (response) {
            if (response.ok) {
                var item = document.querySelector('[data-deck-name="' + deckName + '"]');
                if (item) item.remove();
                syncEmptyState();
                showToast('Deck deleted.', 'success');
            } else {
                showToast('Error deleting deck.', 'error');
            }
        })
        .catch(function (error) {
            showToast('Error deleting deck.', 'error');
        });
}

// ---- New deck ----
function showNewDeckInput(deckName) {
    var newDeckName = prompt("Enter new deck name:", deckName);
    if (newDeckName !== null) {
        newDeck(newDeckName);
    }
}

function newDeck(newDeckName) {
    fetch('/api/decks/new', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newDeckName)
    })
        .then(function (response) {
            if (response.ok) {
                var list = getDeckList();
                if (list) {
                    list.insertAdjacentHTML('beforeend', createDeckItemHtml(newDeckName));
                }
                syncEmptyState();
                showToast('Deck created successfully.', 'success');
            } else {
                showToast('Error creating new deck.', 'error');
            }
        })
        .catch(function (error) {
            showToast('Error creating new deck.', 'error');
        });
}
