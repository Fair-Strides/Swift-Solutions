import { getCountries, getStates, getCities } from './getSearchLocationOptions.js';

// Whether the search bar is currently searching
let isSearching = false;

/**
 * Purpose: Load the search bar with the countries, states, and cities.
 * @returns {Promise<void>}
 * @see PopNGo/wwwroot/js/util/getSearchLocationOptions.js
 */
export async function loadSearchBar() {
    // Load up the countries, states, and cities for the search input
    const countries = await getCountries();
    const countrySelect = document.getElementById('search-event-country');
    const stateSelect = document.getElementById('search-event-state');

    // Populate the country select
    countrySelect.innerHTML = '';
    countries.forEach(country => {
        const option = document.createElement('option');
        option.value = country;
        option.textContent = country;
        countrySelect.appendChild(option);
    });

    // Whenever the country changes, repopulate the states
    countrySelect.addEventListener('change', async () => await updateStates());

    // Whenever the state changes, repopulate the cities
    stateSelect.addEventListener('change', async () => await updateCities());
}

/**
 * Purpose: Get the search query from the search bar.
 * @returns {string}
 */
export function getSearchQuery() {
    const input = document.getElementById('search-event-input')?.value ?? '';
    const city = document.getElementById('search-event-city').value;
    const state = document.getElementById('search-event-state').value;
    const country = document.getElementById('search-event-country').value;

    if(state === 'No states') return `${input} in ${city}, ${country}`;
    return `${input} in ${city}, ${state}, ${country}`;
}

/**
 * Purpose: Toggle the no events section's visiblitity.
 */ 
export function toggleNoEventsSection(show) {
    document.getElementById('no-events-section')?.classList.toggle('hidden', !show);
}

/**
 * Purpose: Toggle the searching events section's visiblitity.
 */
export function toggleSearchingEventsSection(show) {
    document.getElementById('searching-events-section')?.classList.toggle('hidden', !show);
}

/**
 * Purpose: Set the country in the search bar to the provided country.
 * @param {string} country
 * @returns {Promise<void>}
 */
export async function setCountry(country) {
    if(!country) return;
    const countrySelect = document.getElementById('search-event-country');
    const optionExists = Array.from(countrySelect.options).some(option => option.value === country);
    if(!optionExists) return;
    
    // Set the country, then refresh the values for the State.
    countrySelect.value = country;

    // Refresh the states
    await updateStates();
}

/**
 * Purpose: Set the state in the search bar to the provided state.
 * @param {string} state
 * @returns {Promise<void>}
 */
export async function setState(state) {
    if(!state) return;
    const stateSelect = document.getElementById('search-event-state');
    const optionExists = Array.from(stateSelect.options).some(option => option.value === state);
    if(!optionExists) return;
    
    // Set the state, then refresh the values for the City.
    stateSelect.value = state;
    
    // Refresh the cities
    await updateCities();
}

/**
 * Purpose: Set the city in the search bar to the provided city.
 * @param {string} city
 */
export function setCity(city) {
    document.getElementById('search-event-city').value = city;
}

/**
 * Purpose: Update the states in the search bar.
 * @returns {Promise<void>}
 */
async function updateStates() {
    let countrySelect = document.getElementById('search-event-country');
    let stateSelect = document.getElementById('search-event-state');

    const states = await getStates(countrySelect.value);
    stateSelect.innerHTML = '';
    if(states.length === 0) {
        const option = document.createElement('option');
        option.value = '';
        option.textContent = 'No states';
        stateSelect.appendChild(option);
        return;
    } else {
        states.forEach(state => {
            const option = document.createElement('option');
            option.value = state;
            option.textContent = state;
            stateSelect.appendChild(option);
        });
    }

    // Refresh the cities
    await updateCities();
}

/**
 * Purpose: Update the cities in the search bar.
 * @returns {Promise<void>}
 */
async function updateCities() {
    let countrySelect = document.getElementById('search-event-country');
    let stateSelect = document.getElementById('search-event-state');
    let citySelect = document.getElementById('search-event-city');
    
    // Refresh the cities
    const cities = await getCities(countrySelect.value, stateSelect.value);

    citySelect.innerHTML = '';
    cities.forEach(city => {
        const option = document.createElement('option');
        option.value = city;
        option.textContent = city;
        citySelect.appendChild(option);
    });
}

/**
 * Purpose: Toggle the searching state.
 */
export function toggleSearching() {
    isSearching = !isSearching;
}

/**
 * Purpose: Check if the search bar is currently searching.
 * @returns {boolean}
 */
export function isCurrentlySearching() {
    return isSearching;
}