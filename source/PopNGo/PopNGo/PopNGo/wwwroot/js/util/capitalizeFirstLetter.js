/**
 * Capitalize the first letter of a string
 * @param {any} str
 * @returns {string}
 */
export function capitalizeFirstLetter(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}