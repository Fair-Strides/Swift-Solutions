export async function getNearestCityAndState(lat, lng) {
    console.log("GET NEAREST CITY AND STATE", lat, lng)
    const getApiKeyResponse = await fetch('/api/MapApi/GetGeolocationApiKey');
    const apiKey = await getApiKeyResponse.text();

    const response = await fetch(`https://maps.googleapis.com/maps/api/geocode/json?latlng=${lat},${lng}&key=${apiKey}`);

    const data = await response.json();
    const results = data.results;

    if (results && results.length > 0) {
        const addressComponents = results[0].address_components;
        const cityComponent = addressComponents.find(component => component.types.includes('locality'));
        const stateComponent = addressComponents.find(component => component.types.includes('administrative_area_level_1'));

        if (cityComponent && stateComponent) {
            return {
                city: cityComponent.long_name,
                state: stateComponent.long_name
            };
        }
    }

    console.error('Could not find city and state for the provided latitude and longitude');
    return null;
}