import { getNearestCityAndState } from './getNearestCityAndState.js';
import { getEvents } from '../api/events/getEvents.js';

let lastLocation = null;
let isWaiting = false;

export function updateLocationAndFetch(map) {
    if (isWaiting) return;
    isWaiting = true;

    var center = map.getCenter();
    var latitude = center.lat();
    var longitude = center.lng();
    getNearestCityAndState(latitude, longitude).then(location => {
        if (location && (!lastLocation || location.city !== lastLocation.city || location.state !== lastLocation.state)) {
            getEvents(`Events in ${location.city}, ${location.state}`)
                .then(events => {
                    initMap(events);
                    lastLocation = location;
                    console.log(lastLocation, latitude, longitude)

                })
                .catch(error => console.error('Error fetching events:', error));
        } else if (!location) {
            console.log('Could not find city and state for the provided latitude and longitude');
        }
        isWaiting = false;
    });
}

export let debounceTimeout = null;

export function debounceUpdateLocationAndFetch(map) {
    clearTimeout(debounceTimeout);
    debounceTimeout = setTimeout(() => updateLocationAndFetch(map), 500);
}