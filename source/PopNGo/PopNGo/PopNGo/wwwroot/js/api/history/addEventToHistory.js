// Adds event to user's history

/*  Event:
    public int id
    public string apiEventID
    public DateTime eventDate
    public string eventName
    public string eventDescription
    public string eventLocation
}*/

export async function addEventToHistory(event) {
    let url = "/api/EventHistoryApi/EventHistory";
    const res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)
    })

    if (!res.ok) {
        if (res.status === 401) {
            throw new Error('Unauthorized');
        }

        throw new Error('Network response was not ok');
    }
}
