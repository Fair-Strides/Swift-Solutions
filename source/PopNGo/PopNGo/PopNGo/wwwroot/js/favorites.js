import { formatStartTime } from './util/formatStartTime.js';


function fetchUserFavorites() {
    fetch('/api/FavoritesApi/GetUserFavorites')
        .then(response => {
            if (response.status === 401) {
                // Unauthorized, tell the user to log in or sign up
                document.getElementById('login-prompt').style.display = 'block';
                throw new Error('Unauthorized');
            } else if (response.status === 404) {
                // Resource not found, tell the user they have no favorites
                document.getElementById('no-favorites-message').style.display = 'block';
                throw new Error('No favorites found');
            }
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            data.forEach(event => {
                const eventCard = constructEventCard(event);
                document.getElementById('favorite-events-container').appendChild(eventCard);
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function constructEventCard(event) {
    const eventCard = document.createElement('div');
    eventCard.classList.add('event-card');

    // const eventImage = document.createElement('img');
    // eventImage.src = event.eventThumbnail;
    // eventImage.alt = event.eventName;
    // eventImage.classList.add('event-image');
    // eventCard.appendChild(eventImage);

    const eventInfo = document.createElement('div');
    eventInfo.classList.add('event-info');
    eventCard.appendChild(eventInfo);

    const eventName = document.createElement('h2');
    eventName.textContent = event.eventName;
    eventInfo.appendChild(eventName);

    const eventDescription = document.createElement('p');
    eventDescription.textContent = event.eventDescription;
    eventInfo.appendChild(eventDescription);

    const eventDate = document.createElement('p');
    eventDate.textContent = formatStartTime(event.eventDate);
    eventInfo.appendChild(eventDate);

    return eventCard;
}

document.addEventListener('DOMContentLoaded', function () {
    console.log("Favorites page loaded.")
    fetchUserFavorites();
});