/**
 * Load the Google Maps API script
 * @async
 * @function loadMapScript
 * @returns {Promise<void>}
 */
export async function loadMapScript() {
    // Fetch the API key from the server
    const response = await fetch('/api/MapApi/GetApiKey');
    const apiKey = await response.text();
    var script = document.createElement('script');
    script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&loading=async&callback=initMap&libraries=maps,marker&v=beta`;
    script.async = true;
    document.body.appendChild(script); // Append the script to the body instead of the head
}