/**
 * Takes in an api event id and returns a boolean indicating if the event is favorited
 * @async
 * @function getEventIsFavorited
 * @param {String} apiEventId
 * @returns {Promise<boolean>}
 */
export async function getEventIsFavorited(apiEventId) {
    let res = await fetch(`/api/FavoritesApi/IsFavorite?eventId=${apiEventId}`)
    return await res.json();
}