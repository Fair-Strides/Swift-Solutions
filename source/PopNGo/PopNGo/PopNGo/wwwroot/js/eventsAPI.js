//Uses plain JavasScript's fetch to get the REST API.
//Only fetches data and does not manipulate DOM to view data

//Data return example:
//Array(10)[{… }, {… }, {… }, {… }, {… }, {… }, {… }, {… }, {… }, {… }]
//0: Object { eventName: "Deerhoof", eventDescription: "DEERHOOF \\*MIRACLE-LEVEL TOUR\\*\n\nAFTER 28 YEARS, DEERHOOF RECORDS THEIR STUDIO DEBUT AND IT’S ALL IN JAPANESE", eventStartTime: "2024-02-15T21:45:00", … }
//eventDescription: "DEERHOOF \\*MIRACLE-LEVEL TOUR\\*\n\nAFTER 28 YEARS, DEERHOOF RECORDS THEIR STUDIO DEBUT AND IT’S ALL IN JAPANESE"
//eventEndTime: "2024-02-16T03:45:00"
//eventIsVirtual: true
//eventLanguage: "en"
//eventLink: null
//eventName: "Deerhoof"
//eventStartTime: "2024-02-15T21:45:00"
//eventThumbnail: "https://dice-media.imgix.net/attachments/2023-06-05/1ae87a1a-92dd-45e5-bd62-afe41ffad83a.jpg?rect=0%2C0%2C3000%2C3000"
//full_Address: null
//latitude: 41.903908
//longitude: 12.538744
//phone_Number: "+393515211938"

export async function fetchEventData(query) {
    try {
        const response = await fetch(`/api/search/events?q=${query}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function fetchTagId(tag) {
    try {
        const response = await fetch(`/api/tags/name=${tag}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function createTags(tagList) {
    if(tagList.length === 0) return;

    try {
        const response = await fetch(`/api/tags/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(tagList)
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}