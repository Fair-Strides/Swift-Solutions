import { createTags, formatTags } from './util/tags.js';
import { showLoginSignupModal } from './util/showUnauthorizedLoginModal.js';
import { addEventToHistory } from './api/history/addEventToHistory.js';
import { showToast } from './util/toast.js';
import { buildEventCard, validateBuildEventCardProps } from './ui/buildEventCard.js';
import { buildEventDetailsModal, validateBuildEventDetailsModalProps } from './ui/buildEventDetailsModal.js';
import { getEvents } from './api/events/getEvents.js';
import { getEventIsFavorited } from './api/favorites/getEventIsFavorited.js';
import { removeEventFromFavorites } from './api/favorites/removeEventFromFavorites.js';
import { addEventToFavorites } from './api/favorites/addEventToFavorites.js';
import { loadMapScript } from './util/loadMapScript.js';
import { getLocationCoords } from './util/getSearchLocationCoords.js';
import { loadSearchBar, getSearchQuery, toggleNoEventsSection, toggleSearchingEventsSection,
        setCity, setCountry, setState, toggleSearching } from './util/searchBarEvents.js';
import { debounceUpdateLocationAndFetch } from './util/mapFetching.js';
import { getNearestCityAndState } from './util/getNearestCityAndState.js';

let map = null;
let page = 0;
const pageSize = 10;

// Fetch event data and display it
// Call getLocation when the script is loaded
document.addEventListener("DOMContentLoaded", function (event) {
    getLocation();
});

async function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async function (position) {
            const { city, state } = await getNearestCityAndState(position.coords.latitude, position.coords.longitude);
            await loadSearchBarAndEvents(city, state);
        }, async function (error) {
            if (error.code === error.PERMISSION_DENIED) {
                const defaultCity = "Las Vegas";
                const defaultState = "Nevada";

                await loadSearchBarAndEvents(defaultCity, defaultState);
            }
        });
    } else {

    }
}

async function loadSearchBarAndEvents(city, state) {
    await loadSearchBar().then(async () => {
        await setCountry("United States");
        await setState(state);
        setCity(city);
    });

    document.getElementById('next-page-button').addEventListener('click', nextPage);
    document.getElementById('previous-page-button').addEventListener('click', previousPage);

    if (document.getElementById('events-container')) {
        searchForEvents();
    }

    document.getElementById('search-event-button').addEventListener('click', async function () { await searchForEvents(); });

    document.getElementById('search-event-input').addEventListener('keyup', async function (event) {
        if (event.key === 'Enter') {
            await searchForEvents();
        }
    });
}

/**
 * Takes in an event info object and adds it to the history via http and opens the event details modal
 * @async
 * @function onClickDetailsAsync
 * @param {any} eventInfo
 */
async function onClickDetailsAsync(eventInfo) {
    let eventApiBody = {
        ApiEventID: eventInfo.eventID || "No ID available",
        EventDate: eventInfo.eventStartTime || "No date available",
        EventName: eventInfo.eventName || "No name available",
        EventDescription: eventInfo.eventDescription || "No description available",
        EventLocation: eventInfo.full_Address || "No location available",
    };

    const eventDetailsModalProps = {
        img: eventInfo.eventThumbnail,
        title: eventInfo.eventName,
        description: (eventInfo.eventDescription ?? 'No description') + '...',
        date: new Date(eventInfo.eventStartTime),
        fullAddress: eventInfo.full_Address,
        tags: await formatTags(eventInfo.eventTags),
        favorited: await getEventIsFavorited(eventInfo.eventID),
        onPressFavorite: () => onPressFavorite(eventApiBody, eventDetailsModalProps.favorited)
    }

    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();

        addEventToHistory(eventApiBody);
    };
}

/**
 * Get the next page of events and display them
 * @async
 * @function nextPage
 * @returns {Promise<void>}
 */
async function nextPage() {
    window.scrollTo(0, 0);
    page++;
    searchForEvents();

    document.getElementById('page-number').innerHTML = page + 1

    document.getElementById('previous-page-button').innerHTML = page;
    document.getElementById('next-page-button').innerHTML = page + 2;
    document.getElementById('previous-page-button').disabled = false;
}

/**
 * Get the previous page of events and display them
 * @async
 * @function previousPage
 * @returns {Promise<void>}
 */
async function previousPage() {
    if (page > 0) {
        window.scrollTo(0, 0);
        page--;
        searchForEvents();
        // set page number
        document.getElementById('page-number').innerHTML = page + 1;
        document.getElementById('previous-page-button').innerHTML = page;
        document.getElementById('next-page-button').innerHTML = page + 2;
        if (page === 0) {
            document.getElementById('previous-page-button').disabled = true;
        }
    }
}

export function getPaginationIndex() {
    return (page * pageSize);
}

/**
 * Display events.
 * Events is an array of event objects returned from the API
 * @param {any} events
 */
export async function displayEvents(events) {
    let eventsContainer = document.getElementById('events-container')
    eventsContainer.innerHTML = ''; // Clear the container
    let eventCardTemplate = document.getElementById('event-card-template')

    const eventTags = events.map(event => event.eventTags).flat().filter(tag => tag)
    await createTags(eventTags);

    for (let eventInfo of events) {
        let newEventCard = eventCardTemplate.content.cloneNode(true);

        let eventApiBody = {
            ApiEventID: eventInfo.eventID || "No ID available",
            EventDate: eventInfo.eventStartTime || "No date available",
            EventName: eventInfo.eventName || "No name available",
            EventDescription: eventInfo.eventDescription || "No description available",
            EventLocation: eventInfo.full_Address || "No location available",
        };

        // TODO: add validation
        let eventCardProps = {
            img: eventInfo.eventThumbnail,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventStartTime),
            city: eventInfo.full_Address.split(',')[1],
            state: eventInfo.full_Address.split(',')[2],
            tags: await formatTags(eventInfo.eventTags),
            favorited: await getEventIsFavorited(eventInfo.eventID),
            onPressFavorite: () => onPressFavorite(eventApiBody, eventCardProps.favorited),
            onPressEvent: () => onClickDetailsAsync(eventInfo),
        }
        if (validateBuildEventCardProps(eventCardProps)) {
            buildEventCard(newEventCard, eventCardProps);
            eventsContainer.appendChild(newEventCard);
        }
    }
}

/**
 * Takes in an apiEventId, and a favorite status, and updates the favorite status of the event via http
 * 
 * eventApiBody: {
        ApiEventID: eventInfo.eventID || "No ID available",
        EventDate: eventInfo.eventStartTime || "No date available",
        EventName: eventInfo.eventName || "No name available",
        EventDescription: eventInfo.eventDescription || "No description available",
        EventLocation: eventInfo.full_Address || "No location available",
    };
 * 
 * @async
 * @function onPressFavorite
 * @param {object} eventApiBody
 * @param {any} favorited
 * @returns {Promise<void>}
 */
async function onPressFavorite(eventInfo, favorited) {
    if (favorited) {
        removeEventFromFavorites(eventInfo).catch((error) => {
            // TODO: check that it is an unauthorized error
            // Unauthorized, show the login/signup modal
            showLoginSignupModal();
        })
        showToast('Event unfavorited!');
    } else {
        addEventToFavorites(eventInfo).catch((error) => {
            // TODO: check that it is an unauthorized error
            // Unauthorized, show the login/signup modal
            showLoginSignupModal();
        })
        showToast('Event favorited!');
    }
}

/**
 * Run a search for events and display them
 * 
 * @async
 * @function searchForEvents
 * @returns {Promise<void>}
 */
async function searchForEvents() {
    console.log("search")
    toggleNoEventsSection(false);
    toggleSearchingEventsSection(true);
    toggleSearching();

    const events = await getEvents(getSearchQuery(), getPaginationIndex());
    toggleSearchingEventsSection(false); // Hide the searching events section
    if (!events || events.length === 0) {
        toggleNoEventsSection(true);
    }
    displayEvents(events);
    initMap(events);

    const country = document.getElementById('search-event-country').value;
    const state = document.getElementById('search-event-state').value;
    const city = document.getElementById('search-event-city').value;
    let mapCoords = await getLocationCoords(country, state, city);
    console.debug("Coords: ", mapCoords);
    if(map)
        map.setCenter(mapCoords ?? map.getCenter());

    toggleSearching();
}



// Function to create the map and display events
window.initMap = async function (events) {
    var monmouth = { lat: 44.848, lng: -123.229 }; //Hardcoded Monmouth, Oregon coordinates for now

    // Check if the map already exists
    if (!map) {
        // If it doesn't, create a new one
        map = new google.maps.Map(document.getElementById('demo-map-id'), {
            center: monmouth,
            zoom: 10,
            minZoom: 10,
            maxZoom: 15
        });

        google.maps.event.addListener(map, 'idle', () => debounceUpdateLocationAndFetch(map, getPaginationIndex()));
    }

    events.forEach(async eventInfo => {
        // Add a marker on the map for the event
        if (eventInfo) {
            const lat = eventInfo.latitude ? eventInfo.latitude : 44.848; //Hardcoded Monmouth, Oregon coordinates for now
            const lng = eventInfo.longitude ? eventInfo.longitude : -123.229; //Hardcoded Monmouth, Oregon coordinates for now
            const position = { lat, lng };
            const marker = new google.maps.Marker({
                position,
                map,
                title: eventInfo.eventName
            });

            marker.addListener('click', async function () {
                onClickDetailsAsync(eventInfo);
            });
        }
    });
}

window.onload = async function () {
    if (document.getElementById('demo-map-id')) {
        loadMapScript();
    }
}


//async function getLocation() {
//    const x = document.getElementById("demo");

//    if (navigator.geolocation) {
//        navigator.geolocation.getCurrentPosition(
//            async (position) => {
//                x.innerHTML = "Latitude: " + position.coords.latitude +
//                    "<br>Longitude: " + position.coords.longitude;

//                try {
//                    const { city, state } = await getNearestCityAndState(position.coords.latitude, position.coords.longitude);
//                    console.log("the city:", city);
//                    console.log("the state:", state);

//                    // Update input fields with retrieved city and state
//                    setCity(city);
//                    setState(state);

//                    // Update the city input field
//                    // Run the event search function after updating the input fields
//                    await searchForEvents();

//                    // Show pop-up message
//                    alert('Geolocation retrieved successfully!');
//                } catch (error) {
//                    console.error('Error fetching city and state:', error);
//                    // Handle error gracefully
//                    alert('Error fetching city and state. Please try again later.');
//                }
//            },
//            (error) => {
//                handleGeolocationError(error);
//            }
//        );
//    } else {
//        x.innerHTML = "Geolocation is not supported by this browser.";
//        // Handle unsupported browser scenario
//        alert('Geolocation is not supported by this browser.');
//    }
//}

//function handleGeolocationError(error) {
//    // Handle geolocation error
//    switch (error.code) {
//        case error.PERMISSION_DENIED:
//            alert("User denied the request for geolocation.");
//            break;
//        case error.POSITION_UNAVAILABLE:
//            alert("Location information is unavailable.");
//            break;
//        case error.TIMEOUT:
//            alert("The request to get user location timed out.");
//            break;
//        case error.UNKNOWN_ERROR:
//            alert("An unknown error occurred.");
//            break;
//    }
//}

//// Call getLocation function when DOM content is loaded
//document.addEventListener('DOMContentLoaded', getLocation);

//const x = document.getElementById("demo");

//// Call getLocation when the script is loaded
//document.addEventListener("DOMContentLoaded", function (event) {
//    getLocation();
//});

//function getLocation() {
//    if (navigator.geolocation) {
//        navigator.geolocation.getCurrentPosition(showPosition);
//    } else {
//        x.innerHTML = "Geolocation is not supported by this browser.";
//    }
//}

//function showPosition(position) {
//    x.innerHTML = "Latitude: " + position.coords.latitude +
//        "<br>Longitude: " + position.coords.longitude;
//}
