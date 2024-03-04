// Purpose: Contains the javascript functions for the history page.

// Fetch event data and display it on page load
document.addEventListener('DOMContentLoaded', function () {
    console.log("History page loaded.")
    fetchAndDisplayEvents();
});

/*
constructEventCard creates a html element based on the event object passed to it into this template:
<template id="event-card-template">
    <div class="row text-dark mb-3">
        <div class="col-12">
            <div class="card border-0">
                <div class="card-body" id="event-card-body">
                    <h5 class="card-title" id="event-name">Event Name</h5>
                    <p class="card-text" id="event-description">Event Description</p>
                    <p class="card-text" id="event-datetime">Event Date and time</p>
                    <p class="card-text" id="event-location">Event Location</p>
                </div>
            </div>
        </div>
    </div>
</template>*/
function constructEventCard(event) {
    const template = document.getElementById('event-card-template');
    const eventCard = template.content.cloneNode(true);
    eventCard.querySelector('#event-name').textContent = event.eventName || 'Event Name Not Available';
    eventCard.querySelector('#event-description').textContent = event.eventDescription || 'No description available.';
    eventCard.querySelector('#event-datetime').textContent = event.eventDate || 'Event date not found';
    eventCard.querySelector('#event-location').textContent = event.eventLocation || 'Location information not available';
    return eventCard;
}

// Append event cards to the container element
function displayEvents(events) {
    const container = document.getElementById('event-history-card-container');
    if (!container) {
        console.error('Container element #event-history-card-container not found.');
        return;
    }

    // Clear the container
    container.innerHTML = '';

    // Append event cards to the container
    events.forEach(event => {
        const eventCard = constructEventCard(event);
        container.appendChild(eventCard);
    });
}

// Fetch event data and display it
async function fetchAndDisplayEvents() {
    fetch("/api/EventHistoryApi/EventHistory").then(response => response.json())
        .then(data => displayEvents(data))
        .catch(error => console.error('Error fetching event data:', error));
}