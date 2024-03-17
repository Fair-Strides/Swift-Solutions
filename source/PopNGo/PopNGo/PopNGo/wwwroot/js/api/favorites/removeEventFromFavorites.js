export async function removeEventFromFavorites(event) {
    let url = "/api/FavoritesApi/RemoveFavorite";
    let res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)
    })

    if (!res.ok) {
        throw new Error(res.status)
    }
}
