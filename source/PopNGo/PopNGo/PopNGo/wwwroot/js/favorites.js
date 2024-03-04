import { formatStartTime } from './Helper-Functions/formatStartTime.js';

function formatDateWithWeekday(startTime) {
    const date = new Date(startTime);
    return date.toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric' });
}

function formatHourMinute(startTime) {
    const date = new Date(startTime);
    return date.toLocaleTimeString('en-US', { hour: 'numeric', minute: 'numeric' });
}

let favoriteCount = 0; // Initialize the favorite events count to 0

function fetchUserFavorites() {
    fetch('/api/FavoritesApi/GetUserFavorites')
        .then(response => {
            if (response.status === 401) {
                // Unauthorized, tell the user to log in or sign up
                document.getElementById('favorite-events-title').style.display = 'none'; // Hide the title if the user is not logged in
                document.getElementById('login-prompt').style.display = 'block';
                throw new Error('Unauthorized');
            } else if (response.status === 404) {
                // Resource not found, tell the user they have no favorites
                document.getElementById('favorite-events-title').style.display = 'none';
                document.getElementById('no-favorites-message').style.display = 'block';
                throw new Error('No favorites found');
            }
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            data.forEach(event => {
                const eventCard = constructEventCard(event);
                document.getElementById('favorite-events-container').appendChild(eventCard);
                favoriteCount++;
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function constructEventCard(event) {

    const template = document.getElementById('event-card-template');
    const card = template.content.cloneNode(true);

    const titleElement = card.querySelector('.card-title');
    const dateElement = card.querySelector('.card-text:first-of-type small');
    const timeElement = card.querySelector('.card-text:nth-of-type(2) small');
    const descriptionElement = card.querySelector('.card-text:nth-of-type(3)');
    const imgElement = card.querySelector('img');
    const oldHeartButton = card.querySelector('.heart-button');
    const heartButton = oldHeartButton.cloneNode(true); // Clone the heart button to remove all existing event listeners
    oldHeartButton.parentNode.replaceChild(heartButton, oldHeartButton); // Replace the old heart button with the new one



    titleElement.textContent = event.eventName;
    dateElement.textContent = formatDateWithWeekday(event.eventDate);
    timeElement.textContent = formatHourMinute(event.eventDate);
    descriptionElement.textContent = event.eventDescription;
    // imgElement.src = event.eventImage || 'https://via.placeholder.com/1000'; 

    heartButton.addEventListener('click', () => {
        fetch(`/api/FavoritesApi/RemoveFavorite`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ApiEventID: event.apiEventID || "No ID available",
                EventDate: event.eventDate || "No date available",
                EventName: event.eventName || "No name available",
                EventDescription: event.eventDescription || "No description available",
                EventLocation: event.full_Address || "No location available",
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                // Remove the event card from the DOM
                const cardElement = heartButton.closest('.card'); // Find the ancestor card element
                cardElement.remove();
                favoriteCount--;
                if (favoriteCount === 0) { // If there are no more favorite events, show the no-favorites-message
                    document.getElementById('favorite-events-title').style.display = 'none';
                    document.getElementById('no-favorites-message').style.display = 'block';
                }
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    });

    return card;
}

document.addEventListener('DOMContentLoaded', function () {
    console.log("Favorites page loaded.")
    fetchUserFavorites();
});