import { fetchEventData, createTags, fetchTagId } from './eventsAPI.js';

async function processArray(array, asyncFunction) {
    // map array to promises
    const promises = array.map(asyncFunction);
    // wait until all promises resolve
    await Promise.all(promises);
}

function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

// Function to display events
async function displayEvents(events) {
    const container = document.getElementById('eventsContainer');
    if (!container) {
        console.error('Container element #eventsContainer not found.');
        return;
    }

    let tagList = new Set();
    events?.forEach(event => {
        event.eventTags?.forEach(tag => {
            tag = capitalize(tag).replace(/-|_/g, ' ');
            tagList.add(tag);
        });
    });
    await createTags(Array.from(tagList));

    processArray(events, async event => {
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
            processArray(event.eventTags, async (tag) => {
                tag = capitalize(tag).replace(/-|_/g, ' ');

                const tagEl = document.createElement('span');
                tagEl.classList.add('tag');

                let tagIndex = await fetchTagId(tag) || null;
                if(tagIndex !== null)
                    tagEl.classList.add(`tag-${tagIndex}`);

                tagEl.textContent = tag;
                tags.appendChild(tagEl);
            }).then(() => {
                // Grab the children we just made
                let children = Array.prototype.slice.call(tags.children);

                // Sort the children elements
                children.sort((a, b) => a.textContent.localeCompare(b.textContent));

                // Append each child back to tags
                children.forEach(child => tags.appendChild(child));
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
        fetchEventData("Events in Monmouth, Oregon").then(data => {
            displayEvents(data); // Assuming the data structure includes an array in data.data
        }).catch(e => {
            console.error('Fetching events failed:', e);
        });
    }
});