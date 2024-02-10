// Write your JavaScript code.
import { fetchEvents } from './eventsAPI.js'; // Adjust the path as necessary

function displayEvents(events) {
    const container = document.getElementById('eventsContainer');
    if (!container) {
        console.error('Container element #eventsContainer not found.');
        return;
    }

    events.forEach(event => {
        const eventEl = document.createElement('div');
        eventEl.classList.add('event');

        // Safely handle potentially undefined or null values using fallbacks
        const name = document.createElement('h2');
        name.textContent = event.name || 'Event Name Not Available';

        const dateTime = document.createElement('p');
        dateTime.textContent = `Start: ${event.start_time || 'Unknown Start Time'}, End: ${event.end_time || 'Unknown End Time'}`;

        const location = document.createElement('p');
        location.textContent = event.venue && event.venue.name ? event.venue.name : 'Venue information not available';

        const description = document.createElement('p');
        description.textContent = event.description || 'No description available.';

        const thumbnail = new Image();
        thumbnail.src = event.thumbnail || 'https://yourdomain.com/path/to/default-thumbnail.png'; // Provide an absolute URL to the default thumbnail
        thumbnail.alt = event.name || 'Event Thumbnail';

        // Handle tags if present, else indicate 'No tags available'
        const tags = document.createElement('div');
        tags.classList.add('tags');
        if (event.tags && event.tags.length > 0) {
            event.tags.forEach(tag => {
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

        // Append all elements to the eventEl container
        eventEl.appendChild(name);
        eventEl.appendChild(dateTime);
        eventEl.appendChild(location);
        eventEl.appendChild(description);
        eventEl.appendChild(thumbnail);
        eventEl.appendChild(tags);

        // Finally, append the eventEl to the main container
        container.appendChild(eventEl);
    });
}

document.addEventListener('DOMContentLoaded', function () {
    fetchEvents().then(data => {
        displayEvents(data.data); // Assuming the data structure includes an array in data.data
    }).catch(e => {
        console.error('Fetching events failed:', e);
    });
});




