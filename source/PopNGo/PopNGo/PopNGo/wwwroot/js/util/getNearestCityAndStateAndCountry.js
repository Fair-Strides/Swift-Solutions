export async function getNearestCityAndStateAndCountry(lat, lng) {
    console.log("GET NEAREST CITY, STATE, AND COUNTRY", lat, lng)
    const getApiKeyResponse = await fetch('/api/MapApi/GetGeolocationApiKey');
    const apiKey = await getApiKeyResponse.text();

    const response = await fetch(`https://maps.googleapis.com/maps/api/geocode/json?latlng=${lat},${lng}&key=${apiKey}`);

    const data = await response.json();
    const results = data.results;

    if (results && results.length > 0) {
        const addressComponents = results[0].address_components;
        const cityComponent = addressComponents.find(component => component.types.includes('locality'));
        const stateComponent = addressComponents.find(component => component.types.includes('administrative_area_level_1'));
        const countryComponent = addressComponents.find(component => component.types.includes('country'));

        if (cityComponent && stateComponent && countryComponent) {
            return {
                city: cityComponent.long_name,
                state: stateComponent.long_name,
                country: countryComponent.long_name
            };
        }
    }

    console.error('Could not find city, state, and country for the provided latitude and longitude');
    return null;
}
