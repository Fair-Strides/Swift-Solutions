import { buildEventCard } from "./ui/buildEventCard.js";
import { formatTags } from "./util/tags.js";
import { getEventIsFavorited } from "./api/favorites/getEventIsFavorited.js";
import { removeEventFromFavorites } from './api/favorites/removeEventFromFavorites.js';
import { addEventToFavorites } from './api/favorites/addEventToFavorites.js';
import { showToast } from './util/toast.js';

async function onPressFavorite(eventInfo, favorited) {
    if (favorited) {
        await removeEventFromFavorites(eventInfo).catch((error) => {
            // TODO: check that it is an unauthorized error
            // Unauthorized, show the login/signup modal
            showLoginSignupModal();
        })
        showToast('Event unfavorited!');
        eventInfo.favorited = false; // Update the favorited status
    } else {
        await addEventToFavorites(eventInfo).catch((error) => {
            // TODO: check that it is an unauthorized error
            // Unauthorized, show the login/signup modal
            showLoginSignupModal();
        })
        showToast('Event favorited!');
        eventInfo.favorited = true; // Update the favorited status
    }
}

// Fetch event data and display it on page load
document.addEventListener('DOMContentLoaded', function () {
    console.log("History page loaded.")
    fetchEvents()
        .then(data => {
            displayEvents(data);
            document.getElementById("history-container").style.display = "block";
        })
        .catch(error => {
            if (error.message === 'Unauthorized') {
                // Handle 401 error here
                console.error('Unauthorized: ', error);
            } else {
                console.error('Error fetching event data:', error);
            }
        });

    // Append event cards to the container element
    // Append event cards to the container
    async function displayEvents(events) {
        const container = document.getElementById('event-history-card-container');
        if (!container) {
            console.error('Container element #event-history-card-container not found.');
            return;
        }

        // Clear the container
        container.innerHTML = '';

        // Append event cards to the container
        for (const event of events) {
            // Get the template
            const template = document.getElementById('event-card-template');

            let eventApiBody = {
                ApiEventID: event.apiEventID || "No ID available",
                EventDate: event.eventDate || "No date available",
                EventName: event.eventName || "No name available",
                EventDescription: event.eventDescription || "No description available",
                EventLocation: event.eventLocation || "No location available",
            };

            let eventProps = {
                img: event.eventThumbnail, // This property doesn't exist in the provided JSON object
                title: event.eventName,
                date: new Date(event.eventDate),
                city: event.eventLocation, // This is the full address in my data model
                state: "", // State is not provided in the JSON object
                tags: await formatTags(event.eventTags), // This property doesn't exist in the provided JSON object
                favorited: await getEventIsFavorited(event.apiEventID), // Assuming id is the eventID
                onPressEvent: () => {
                    // define what should happen when the event button is clicked
                },
                onPressFavorite: () => onPressFavorite(eventApiBody, eventProps.favorited)
            };
            
            // Clone the template
            const eventCard = template.content.cloneNode(true);

            // Build the event card
            buildEventCard(eventCard, eventProps);

            // Append the event card to the container
            console.log(eventProps)
            container.appendChild(eventCard);
        }
    }

    // Fetch event data and display it
    async function fetchEvents() {
        const response = await fetch("/api/EventHistoryApi/EventHistory");
        if (!response.ok) {
            if (response.status === 401) {
                document.getElementById("login-prompt").style.display = "block";
                throw new Error('Unauthorized');
            } else if (response.status === 404) {
                document.getElementById("no-history-message").style.display = "block";
                throw new Error('No history found');
            }
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        console.log(data);
        return data;
    }
});