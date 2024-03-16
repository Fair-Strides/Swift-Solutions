import { getNearestCityAndState } from './getNearestCityAndState.js';
import { getEvents } from '../api/events/getEvents.js';

let lastLocation = null;
let isWaiting = false;

/**
 * Purpose: Updates the location and fetches events based on the map's center.
 * @param {any} map
 * @param {any} start - Event offset (should be page * items per page)
 * @returns
 */
export function updateLocationAndFetch(map, start) {
    if (isWaiting) return;
    isWaiting = true;

    var center = map.getCenter();
    var latitude = center.lat();
    var longitude = center.lng();
    getNearestCityAndState(latitude, longitude).then(async location => {
        if (location && (!lastLocation || location.city !== lastLocation.city || location.state !== lastLocation.state)) {
            getEvents(`Events in ${location.city}, ${location.state}`, start)
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

export function debounceUpdateLocationAndFetch(map, start) {
    clearTimeout(debounceTimeout);
    debounceTimeout = setTimeout(() => updateLocationAndFetch(map, start), 500);
}