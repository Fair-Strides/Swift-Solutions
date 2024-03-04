//Uses plain JavasScript's fetch to get the REST API.
//Only fetches data and does not manipulate DOM to view data

//Data return example:
//Array(10)[{… }, {… }, {… }, {… }, {… }, {… }, {… }, {… }, {… }, {… }]
//0: Object { eventName: "Deerhoof", eventDescription: "DEERHOOF \\*MIRACLE-LEVEL TOUR\\*\n\nAFTER 28 YEARS, DEERHOOF RECORDS THEIR STUDIO DEBUT AND IT’S ALL IN JAPANESE", eventStartTime: "2024-02-15T21:45:00", … }
//eventDescription: "DEERHOOF \\*MIRACLE-LEVEL TOUR\\*\n\nAFTER 28 YEARS, DEERHOOF RECORDS THEIR STUDIO DEBUT AND IT’S ALL IN JAPANESE"
//eventEndTime: "2024-02-16T03:45:00"
//eventIsVirtual: true
//eventLanguage: "en"
//eventLink: null
//eventName: "Deerhoof"
//eventStartTime: "2024-02-15T21:45:00"
//eventThumbnail: "https://dice-media.imgix.net/attachments/2023-06-05/1ae87a1a-92dd-45e5-bd62-afe41ffad83a.jpg?rect=0%2C0%2C3000%2C3000"
//full_Address: null
//latitude: 41.903908
//longitude: 12.538744
//phone_Number: "+393515211938"

export async function fetchEventData(query) {
    try {
        const response = await fetch(`/api/search/events?q=${query}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function searchForEvents(query, callback) {
    const searchQuery = query ?? document.getElementById('search-event-input').value;
    document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Hide the no events section
    document.getElementById('searching-events-section')?.classList.toggle('hidden', false); // Show the searching events section

    if (searchQuery) {
        fetchEventData(searchQuery).then(data => {
            callback(data); // Assuming the data structure includes an array in data.data
        }).catch(e => {
            console.error('Fetching events failed:', e);
        });
    }
}

export async function fetchTagColor(tag) {
    try {
        const response = await fetch(`/api/tags/name=${tag}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function createTags(events) {
    if(events.length === 0) return;

    // Create a set of unique tags across the events
    let tagList = new Set();
    events?.forEach(event => {
        event.eventTags?.forEach(tag => {
            tag = capitalize(tag).replace(/-|_/g, ' ');
            tagList.add(tag);
        });
    });

    if(tagList.length === 0) return;

    // Create the tags
    try {
        const response = await fetch(`/api/tags/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Array.from(tagList))
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function formatTags(tags, tagsParent) {
    processArray(tags, async (tag) => {
        tag = capitalize(tag).replace(/-|_/g, ' ');

        const tagEl = document.createElement('span');
        tagEl.classList.add('tag');

        let tagStyle = await fetchTagColor(tag) || null;
        if(tagStyle !== null) {
            tagEl.style.backgroundColor = tagStyle.backgroundColor;
            tagEl.style.color = tagStyle.textColor;
        }

        tagEl.textContent = tag;
        tagsParent.appendChild(tagEl);
    }).then(() => {
        // Grab the children we just made
        let children = Array.prototype.slice.call(tagsParent.children);

        // Sort the children elements
        children.sort((a, b) => a.textContent.localeCompare(b.textContent));

        // Append each child back to tags
        children.forEach(child => tagsParent.appendChild(child));
    });
}

export async function processArray(array, asyncFunction) {
    // map array to promises
    const promises = array.map(asyncFunction);
    // wait until all promises resolve
    await Promise.all(promises);
}

export function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}