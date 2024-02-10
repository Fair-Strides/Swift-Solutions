// This JS module provides a function to fetch events from the Real-Time Events Search API
export function fetchEvents(query = 'Events in Monmouth, Oregon') {
    const url = `https://real-time-events-search.p.rapidapi.com/search-events?query=${encodeURIComponent(query)}&start=0`;
    const headers = new Headers({
        'X-RapidAPI-Key': 'a7cb55faecmsh1fda4c5b95f4499p1ae3efjsn1147db4ac435',
        'X-RapidAPI-Host': 'real-time-events-search.p.rapidapi.com'
    });

    return fetch(url, { method: 'GET', headers: headers })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json(); // Parse the JSON of the response
        })
        .catch(e => {
            console.error('Error:', e); // Log any errors
            throw e; // Rethrow to allow catching by the caller
        });
}



