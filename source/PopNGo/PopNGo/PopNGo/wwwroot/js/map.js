import { onClickFavorite } from './api/favorites/onClickFavorite.js';
import { searchForEvents, createTags, formatTags } from './eventsAPI.js';
import { formatStartTime } from './util/formatStartTime.js';
import { getNearestCityAndState } from './util/getNearestCityAndState.js';
import { loadMapScript } from './util/loadMapScript.js';
import { getEventIsFavorited } from './api/favorites/getEventIsFavorited.js';

var map;
// Function to create the map and display events
window.initMap = async function (events) {
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    if (!events || events.length === 0) {
        document.getElementById('no-events-section')?.classList.toggle('hidden', false); // Show the no events section
        return;
    } else
        document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Show the no events section

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
    }

    await google.maps.event.addListener(map, 'dragend', function () {
        var center = map.getCenter();
        var latitude = center.lat();
        var longitude = center.lng();
        getNearestCityAndState(latitude, longitude).then(location => {
            if (location) {
                searchForEvents(`Events in ${location.city}, ${location.state}`, initMap)
            } else {
                console.log('Could not find city and state for the provided latitude and longitude');
            }
        });
    });

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
        loadMapScript();
        searchForEvents("Events in Monmouth, Oregon", initMap);
    }

    document.getElementById('search-event-button').addEventListener('click', searchForEvents(null, initMap));

    document.getElementById('search-event-input').addEventListener('keyup', function (event) {
        if (event.key === 'Enter')
            searchForEvents(null, initMap);
    });
};