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
import {
    loadSearchBar, getSearchQuery, toggleNoEventsSection, toggleSearchingEventsSection,
    setCity, setCountry, setState, toggleSearching
} from './util/searchBarEvents.js';
import { debounceUpdateLocationAndFetch } from './util/mapFetching.js';
import { getNearestCityAndState } from './util/getNearestCityAndState.js';
import { getNearestCityAndStateAndCountry } from './util/getNearestCityAndStateAndCountry.js';

let map = null;
let page = 0;
const pageSize = 10;

// Fetch event data and display it
// Call getLocation when the script is loaded
document.addEventListener("DOMContentLoaded", async function () {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async function (position) {
            const { city, state, country } = await getNearestCityAndStateAndCountry(position.coords.latitude, position.coords.longitude);
            await loadSearchBarAndEvents(city, state, country);
        }, async function (error) {
            if (error.code === error.PERMISSION_DENIED) {
                const defaultCountry = "United States";
                const defaultCity = "Las Vegas";
                const defaultState = "Nevada";

                await loadSearchBarAndEvents(defaultCity, defaultState, defaultCountry);
            }
        });
    }
}, { once: true });


async function loadSearchBarAndEvents(city, state, country) {
    await loadSearchBar().then(async () => {
        await setCountry(country);
        await setState(state);
        setCity(city);
    });

    document.getElementById('next-page-button').addEventListener('click', nextPage);
    document.getElementById('previous-page-button').addEventListener('click', previousPage);

    if (document.getElementById('events-container')) {
        await searchForEvents();
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
        EventImage: eventInfo.eventThumbnail
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
    if (map)
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

    events?.forEach(async eventInfo => {
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

            google.maps.event.addListener(map, 'idle', () => debounceUpdateLocationAndFetch(map));
        }
    });
}

window.onload = async function () {
    if (document.getElementById('demo-map-id')) {
        loadMapScript();
    }
}

// Save Location, Remove Location, and Use Location
let savedLocations = JSON.parse(localStorage.getItem('savedLocations')) || [];
function displaySavedLocations() {
    const savedLocationsContainer = document.getElementById('saved-locations-container');
    savedLocationsContainer.innerHTML = '';

    savedLocations.forEach((location, index) => {
        const locationElement = document.createElement('div');
        locationElement.textContent = `${location.city}, ${location.state}, ${location.country}`;

        const removeButton = document.createElement('button');
        removeButton.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                      <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                      <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                                  </svg>`;
        removeButton.classList.add('btn', 'btn-outline-warning', 'remove-button');
        removeButton.addEventListener('click', function () {
            removeLocation(index);
            displaySavedLocations();
        });

        const useLocationButton = document.createElement('button');
        useLocationButton.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-right-circle" viewBox="0 0 16 16">
                                           <path fill-rule="evenodd" d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8m15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0M4.5 7.5a.5.5 0 0 0 0 1h5.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5z"/>
                                       </svg>`;
        useLocationButton.classList.add('btn', 'btn-outline-primary', 'use-button');
        useLocationButton.addEventListener('click', async function () {
            document.getElementById('search-event-country').value = location.country;
            document.getElementById('search-event-state').value = location.state;
            document.getElementById('search-event-city').value = location.city;

            await loadSearchBar();
            await setCountry("United States");
            await setState(location.state);
            setCity(location.city);

            searchForEvents();
        });

        locationElement.appendChild(removeButton);
        locationElement.appendChild(useLocationButton);
        savedLocationsContainer.appendChild(locationElement);
    });
}

displaySavedLocations();
document.getElementById('save-location-button').addEventListener('click', function () {
    saveLocation();
});
function saveLocation() {
    const country = document.getElementById('search-event-country').value;
    const state = document.getElementById('search-event-state').value;
    const city = document.getElementById('search-event-city').value;

    savedLocations.push({ city, state, country });
    localStorage.setItem('savedLocations', JSON.stringify(savedLocations));
    displaySavedLocations();
}
function removeLocation(index) {
    savedLocations.splice(index, 1);
    localStorage.setItem('savedLocations', JSON.stringify(savedLocations));
    displaySavedLocations();
}
