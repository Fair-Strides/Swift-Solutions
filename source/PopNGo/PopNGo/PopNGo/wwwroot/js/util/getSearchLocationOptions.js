
export async function getCountries() {
    try {
        const response = await fetch('https://countriesnow.space/api/v0.1/countries');
        if(!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        if(data.error) {
            throw new Error(data.error);
        } else {
            console.debug(`CountriesNow Response: ${data.msg}`);
        }

        return data.data.map(country => country.country);
    } catch(error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function getStates(country) {
    const locationInfo = {
        country: country
    };
    
    try {
        const response = await fetch('https://countriesnow.space/api/v0.1/countries/states', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(locationInfo)
        });

        if(!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        if(data.error) {
            throw new Error(data.error);
        } else {
            console.debug(`CountriesNow Response: ${data.msg}`);
        }

        return data.data.states.map(state => state.name);
    } catch(error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function getCities(country, state) {
    let locationInfo = {};
    let apiUrl = '';

    if(state !== "No states") {
        locationInfo = {
            country: country,
            state: state
        };
        apiUrl = 'https://countriesnow.space/api/v0.1/countries/state/cities';

    } else {
        locationInfo = {
            country: country
        };

        apiUrl = 'https://countriesnow.space/api/v0.1/countries/cities';
    }
    
    try {
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(locationInfo)
        });

        if(!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        if(data.error) {
            throw new Error(data.error);
        } else {
            console.debug(`CountriesNow Response: ${data.msg}`);
        }

        return data.data;
    } catch(error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}