import { toggleNoEventsSection, toggleSearchingEventsSection } from "../../util/searchBarEvents.js";
/* Data return example:
[
    { 
        eventDescription: "DEERHOOF \\*MIRACLE-LEVEL TOUR\\*\n\nAFTER 28 YEARS, DEERHOOF RECORDS THEIR STUDIO DEBUT AND IT�S ALL IN JAPANESE"
        eventEndTime: "2024-02-16T03:45:00"
        eventIsVirtual: true
        eventLanguage: "en"
        eventLink: null
        eventName: "Deerhoof"
        eventStartTime: "2024-02-15T21:45:00"
        eventThumbnail: "https://dice-media.imgix.net/attachments/2023-06-05/1ae87a1a-92dd-45e5-bd62-afe41ffad83a.jpg?rect=0%2C0%2C3000%2C3000"
        full_Address: null
        latitude: 41.903908
        longitude: 12.538744
        phone_Number: "+393515211938"
    }, 
    {...}, ...
]
*/

/**
 * Purpose: Fetches event data from the server based on query string.
 * 
 * Query string is the search term that the user enters in the search bar,
 * just like searching on Google. Example: "Sports events in Monmouth, Oregon"
 * 
 * Start is the index of the first event to return. This is used for pagination.
 * @async
 * @function getEvents
 * @param {string} query
 * @param {number} start - The index of the first event to return
 * @returns {Object[]}
 */
export async function getEvents(query, start) {
    try {
        const response = await fetch(`/api/search/events?q=${query}&start=${start}`);
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

