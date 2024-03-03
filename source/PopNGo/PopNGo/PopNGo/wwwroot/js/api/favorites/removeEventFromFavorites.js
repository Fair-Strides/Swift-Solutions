export async function removeEventFromFavorites(event) {
    let url = "/api/FavoritesApi/RemoveFavorite";
    await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)
    })
}
