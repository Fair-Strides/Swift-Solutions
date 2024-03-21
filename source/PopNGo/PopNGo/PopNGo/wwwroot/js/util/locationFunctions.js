// locationFunctions.js
import { loadSearchBar, setCity, setCountry, setState } from './searchBarEvents.js';
// Save Location, Remove Location, Use Location
export let savedLocations = JSON.parse(localStorage.getItem('savedLocations')) || [];

export function displaySavedLocations() {
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

export function saveLocation() {
    const country = document.getElementById('search-event-country').value;
    const state = document.getElementById('search-event-state').value;
    const city = document.getElementById('search-event-city').value;

    savedLocations.push({ city, state, country });
    localStorage.setItem('savedLocations', JSON.stringify(savedLocations));
    displaySavedLocations();
}

export function removeLocation(index) {
    savedLocations.splice(index, 1);
    localStorage.setItem('savedLocations', JSON.stringify(savedLocations));
    displaySavedLocations();
}

