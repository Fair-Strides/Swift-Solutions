import { getCountries, getStates, getCities } from './getSearchLocationOptions.js';

export async function loadSearchBar() {
    // Load up the countries, states, and cities for the search input
    const countries = await getCountries();
    const countrySelect = document.getElementById('search-event-country');
    const stateSelect = document.getElementById('search-event-state');
    const citySelect = document.getElementById('search-event-city');

    // Populate the country select
    countrySelect.innerHTML = '';
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
}

export async function getSearchQuery() {
    const input = document.getElementById('search-event-input')?.value ?? '';
    const city = document.getElementById('search-event-city').value;
    const state = document.getElementById('search-event-state').value;
    const country = document.getElementById('search-event-country').value;

    return `${input} in ${city}, ${state}, ${country}`;
}

export function toggleNoEventsSection(show) {
    document.getElementById('no-events-section')?.classList.toggle('hidden', !show);
}

export function toggleSearchingEventsSection(show) {
    document.getElementById('searching-events-section')?.classList.toggle('hidden', !show);
}

export async function setCountry(country) {
    document.getElementById('search-event-country').value = country;
}

export async function setState(state) {
    document.getElementById('search-event-state').value = state;
}

export async function setCity(city) {
    document.getElementById('search-event-city').value = city;
}