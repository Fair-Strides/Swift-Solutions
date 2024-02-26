import { searchForEvents, createTags, processArray, formatTags } from './eventsAPI.js';
import { showLoginSignupModal } from './Helper-Functions/showUnauthorizedLoginModal.js';

// Function to display events
async function displayEvents(events) {
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    const container = document.getElementById('eventsContainer');
    if (!container) {
        console.error('Container element #eventsContainer not found.');
        return;
    } else
        container.innerHTML = ''; // Clear the container

    if (!events || events.length === 0) {
        document.getElementById('no-events-section')?.classList.toggle('hidden', false); // Show the no events section
        return;
    }

    await createTags(events);

    processArray(events, async event => {
        // Create elements for each event and append them to the container
        const heart = new Image();
        heart.alt = 'Favorite/Unfavorite Event';
        heart.classList.add('heart');
        heart.style.cursor = 'pointer'; //might want to add this to css if possible, but i dont think its necessary

        let isFavorite;

        const updateFavoriteStatus = () => {
            let eventInfo = {
                ApiEventID: event.eventID || "No ID available",
                EventDate: event.eventStartTime || "No date available",
                EventName: event.eventName || "No name available",
                EventDescription: event.eventDescription || "No description available",
                EventLocation: event.full_Address || "No location available",
            };

            let url = isFavorite ? "/api/FavoritesApi/RemoveFavorite" : "/api/FavoritesApi/AddFavorite";
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(eventInfo)
            })
                .then(async response => {
                    if (response.status === 401) {
                        // Unauthorized, show the login/signup modal
                        showLoginSignupModal();
                        throw new Error('Unauthorized');
                    }
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    const text = await response.text();
                    return text ? JSON.parse(text) : {};
                })
                .then(() => {
                    // Update the favorite status and the image source
                    isFavorite = !isFavorite;
                    heart.src = isFavorite ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';

                    // Create a toast to show the user that the favorite status has been updated
                    const toast = document.createElement('div');
                    toast.classList.add('toast');
                    toast.textContent = isFavorite ? 'Event favorited!' : 'Event unfavorited!';

                    // Append the toast to the toast container div
                    const toastContainer = document.getElementById('toastContainer');
                    toastContainer.appendChild(toast);

                    // Show the toast
                    toast.classList.add('show');

                    // Remove the toast after 3 seconds
                    setTimeout(() => {
                        toast.classList.remove('show');
                        setTimeout(() => {
                            toastContainer.removeChild(toast);
                        }, 500); // Wait for the transition to finish before removing the toast
                    }, 3000);
                });
        };

        fetch(`/api/FavoritesApi/IsFavorite?eventId=${event.eventID}`)
            .then(response => response.json())
            .then(favoriteStatus => {
                isFavorite = favoriteStatus;
                heart.src = isFavorite ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg'; //THIS IS WHERE THE IMAGE PATHS ARE HARDCODED
                heart.addEventListener('click', updateFavoriteStatus);
            });

        const eventEl = document.createElement('div');
        eventEl.classList.add('event');

        const name = document.createElement('h2');
        name.textContent = event.eventName || 'Event Name Not Available';

        const dateTime = document.createElement('p');
        dateTime.textContent = `Start: ${event.eventStartTime || 'Unknown Start Time'}, End: ${event.eventEndTime || 'Unknown End Time'}`;

        const location = document.createElement('p');
        location.textContent = event.full_Address || 'Location information not available';

        const description = document.createElement('p');
        description.textContent = event.eventDescription || 'No description available.';

        const thumbnail = new Image();
        thumbnail.src = event.eventThumbnail || 'https://yourdomain.com/path/to/default-thumbnail.png';
        thumbnail.alt = event.eventName || 'Event Thumbnail';

        const tags = document.createElement('div');
        tags.classList.add('tags');

        if (event.eventTags && event.eventTags.length > 0) {
            await formatTags(event, tags);
        } else {
            const tagEl = document.createElement('span');
            tagEl.classList.add('tag');
            tagEl.textContent = "No tags available";
            tags.appendChild(tagEl);
        }

        eventEl.appendChild(name);
        eventEl.appendChild(dateTime);
        eventEl.appendChild(location);
        eventEl.appendChild(description);
        eventEl.appendChild(thumbnail);
        eventEl.appendChild(tags);
        eventEl.appendChild(heart);

        container.appendChild(eventEl);
    });
}


// Fetch event data and display it
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Hide the no events section
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    if (document.getElementById('eventsContainer')) {
        searchForEvents("Events in Monmouth, Oregon", displayEvents);
    }

    document.getElementById('search-event-button').addEventListener('click', searchForEvents(null, displayEvents));

    document.getElementById('search-event-input').addEventListener('keyup', function (event) {
        if (event.key === 'Enter')
            searchForEvents(null, displayEvents);
    });
});