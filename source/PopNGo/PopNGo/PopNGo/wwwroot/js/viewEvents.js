import { fetchEventData } from './eventsAPI.js';

// Function to display events
function displayEvents(events) {
    const container = document.getElementById('eventsContainer');
    if (!container) {
        console.error('Container element #eventsContainer not found.');
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
    if (document.getElementById('eventsContainer')) {
        fetchEvents().then(data => {
            displayEvents(data.data); // Assuming the data structure includes an array in data.data
        }).catch(e => {
            console.error('Fetching events failed:', e);
        });
    }
    const query = 'Events in Monmouth, Oregon'; // Modify the query as needed
    const searchTitle = document.getElementById('search-title');

    // Check if the search-title element exists
    if (searchTitle) {
        searchTitle.textContent = query;
    } else {
        console.error('Element #search-title not found.');
    }

    fetchEventData(query)
        .then(data => {
            console.log('Event data:', data);
            displayEvents(data); 
        })
        .catch(error => {
            console.error('Error fetching event data:', error);
        });
});