import { fetchEventData, searchForEvents } from './eventsAPI.js';

// Function to display events
function displayEvents(events) {
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

    events.forEach(event => {
        // Create elements for each event and append them to the container
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
            event.eventTags.forEach(tag => {
                const tagEl = document.createElement('span');
                tagEl.classList.add('tag');
                tagEl.textContent = tag;
                tags.appendChild(tagEl);
            });
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