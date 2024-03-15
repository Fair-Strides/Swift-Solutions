import { onClickFavorite } from './api/favorites/onClickFavorite.js';
import { createTags, formatTags } from './eventsAPI.js';
import { getEvents } from './api/events/getEvents.js';
import { formatStartTime } from './util/formatStartTime.js';
import { loadMapScript } from './util/loadMapScript.js';
import { getEventIsFavorited } from './api/favorites/getEventIsFavorited.js';
import { getCountries, getStates, getCities } from './util/getSearchLocationOptions.js';
import { getLocationCoords } from './util/getSearchLocationCoords.js';
import { debounceUpdateLocationAndFetch } from './util/mapFetching.js';


let map;
// let lastLocation = null; // The last location that was searched for
// let isWaiting = false; // A flag to prevent the updateLocationAndFetch function from being called too frequently

// Function to create the map and display events
window.initMap = async function (events) {
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    if (!events || events.length === 0) {
        document.getElementById('no-events-section')?.classList.toggle('hidden', false); // Show the no events section
        return;
    } else
        document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Show the no events section

    var monmouth = { lat: 44.848, lng: -123.229 }; //Hardcoded Monmouth, Oregon coordinates for now
    const country = document.getElementById('search-event-country').value;
    const state = document.getElementById('search-event-state').value;
    const city = document.getElementById('search-event-city').value;
    let mapCoords = await getLocationCoords(country, state, city);
    console.debug("Coords: ", mapCoords);

    // Check if the map already exists
    if (!map) {
        // If it doesn't, create a new one
        map = new google.maps.Map(document.getElementById('demo-map-id'), {
            center: mapCoords ?? monmouth,
            zoom: 10,
            minZoom: 10,
            maxZoom: 15
        });
      
        google.maps.event.addListener(map, 'idle', () => debounceUpdateLocationAndFetch(map));
    } else {
        // If it does, move the map to the new coordinates
        map.setCenter(mapCoords ?? monmouth);
    }


    const loadingOverlay = document.createElement('div');
    loadingOverlay.id = 'loading-overlay';

    document.getElementById('demo-map-id').appendChild(loadingOverlay);

    let selectedEvent = null;

    document.getElementById('demo-map-id').removeChild(loadingOverlay);

    await createTags(events);
    events.forEach(async event => {
        // Add a marker on the map for the event
        if (event) {
            const lat = event.latitude ? event.latitude : 44.848; //Hardcoded Monmouth, Oregon coordinates for now
            const lng = event.longitude ? event.longitude : -123.229; //Hardcoded Monmouth, Oregon coordinates for now
            const position = { lat, lng };
            const marker = new google.maps.Marker({
                position,
                map,
                title: event.eventName
            });

            let favorited = await getEventIsFavorited(event.eventID);

            marker.addListener('click', async function () {
                var mapDiv = document.getElementById('demo-map-id');
                var eventInfoDiv = document.getElementById('event-info');

                if (selectedEvent === event) {
                    // If the same event is clicked again, reset the map and event info divs to their initial sizes
                    mapDiv.style.width = '100vw';
                    eventInfoDiv.style.width = '0';
                    eventInfoDiv.style.display = 'none';
                    selectedEvent = null;
                } else {
                    // If a different event is clicked, resize the map and event info divs and populate the event info div with the event details
                    mapDiv.style.width = '50vw';
                    eventInfoDiv.style.width = '50vw';

                    var nameElement = document.getElementById('name');
                    var descriptionElement = document.getElementById('description');
                    var dateElement = document.getElementById('date');
                    var imageElement = document.getElementById('image');
                    var tagsElement = document.getElementById('tags');
                    tagsElement.innerHTML = '';

                    // Populate the event info div with the tags
                    if (event.eventTags && event.eventTags.length > 0) {
                        await formatTags(event.eventTags, tagsElement);
                    } else {
                        const tagEl = document.createElement('span');
                        tagEl.classList.add('tag');
                        tagEl.textContent = "No tags available";
                        tagsElement.appendChild(tagEl);
                    }

                    let heartIcon = document.getElementById('heart-icon');

                    let eventInfo = {
                        ApiEventID: event.eventID,
                        EventDate: event.eventStartTime || "No date available",
                        EventName: event.eventName || "No name available",
                        EventDescription: event.eventDescription || "No description available",
                        EventLocation: event.full_Address || "No location available",
                    };

                    heartIcon.src = favorited ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';
                    heartIcon.addEventListener('click', () => onClickFavorite(heartIcon, eventInfo));

                    nameElement.textContent = event.eventName;
                    descriptionElement.textContent = event.eventDescription;
                    dateElement.textContent = formatStartTime(event.eventStartTime);

                    imageElement.src = '/media/images/gifs/loadingspinner.gif';

                    // Add an onload event listener to the image element
                    imageElement.onload = function () {
                        // The image has loaded
                        if (event.eventThumbnail) {
                            imageElement.src = event.eventThumbnail;
                        } else {
                            imageElement.src = '../media/images/400X400_placeholder.png';
                        }
                    };

                    eventInfoDiv.style.display = 'block';

                    setTimeout(function () {
                        google.maps.event.trigger(map, 'resize');
                    }, 500);

                    selectedEvent = event;
                }
            });
        }
    });
}

window.onload = async function () {
    document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Hide the no events section
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    if (document.getElementById('demo-map-id')) {
        await loadMapScript();
        // searchForEvents("Events in Monmouth, Oregon", initMap);
        let events = await getEvents("Events in Monmouth, Oregon");
        initMap(events);
    }

    // Load up the countries, states, and cities for the search input
    const countries = await getCountries();
    const countrySelect = document.getElementById('search-event-country');
    const stateSelect = document.getElementById('search-event-state');
    const citySelect = document.getElementById('search-event-city');

    // Populate the country select
    countries.forEach(country => {
        const option = document.createElement('option');
        option.value = country;
        option.textContent = country;
        countrySelect.appendChild(option);
    });

    // Whenever the country changes, repopulate the states
    countrySelect.addEventListener('change', async function () {
        const states = await getStates(countrySelect.value);
        stateSelect.innerHTML = '';
        states.forEach(state => {
            const option = document.createElement('option');
            option.value = state;
            option.textContent = state;
            stateSelect.appendChild(option);
        });

        stateSelect.dispatchEvent(new Event('change'));
    });

    // Whenever the state changes, repopulate the cities
    stateSelect.addEventListener('change', async function () {
        const cities = await getCities(countrySelect.value, stateSelect.value);
        citySelect.innerHTML = '';
        cities.forEach(city => {
            const option = document.createElement('option');
            option.value = city;
            option.textContent = city;
            citySelect.appendChild(option);
        });
    });

    countrySelect.dispatchEvent(new Event('change'));

    document.getElementById('search-event-button').addEventListener('click', function () { searchForEvents(null, initMap) });

    document.getElementById('search-event-input').addEventListener('keyup', function (event) {
        if (event.key === 'Enter')
            getEvents("Events in Monmouth, Oregon");
    });
};