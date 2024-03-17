import { capitalizeFirstLetter } from "./capitalizeFirstLetter.js";


/**
 * Takes in a list of string tags and returns a list of objects with the tag name and colors:
 * [{
 *    tagName: String,
 *    tagTextColor: String,
 *    tagBackgroundColor: String,
 * }, {...}, ...]
 * @async
 * @function formatTags
 * @param {String[]} tag - The string tag name
 * @returns {Promise<Object[]>} formattedTags
 */
export async function formatTags(tags) {
    if (tags == null || tags.length === 0) return [];
    return await Promise.all(tags.map(formatTag));
}

/**
 * Takes in list of tags strings and creates the tags in the database, if they don't already exist
 * @param {String} tagNames - The list of tag names
 * @returns
 */
export async function createTags(tagNames) {
    if (tagNames == null || tagNames == undefined || tagNames.length === 0) return;

    const formattedTagNames = tagNames.map((tagName) => {
        return formatTagName(tagName)
    })

    // Create the tags
    try {
        const response = await fetch(`/api/tags/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Array.from(formattedTagNames))
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

/**
 * Takes in a string tag and returns the tag name formatted for display
 * 
 * Example:
 * sports_and_stuff -> Sports and stuff
 * @param {String} tag - The string tag name
 * @returns {String} The formatted tag name
 */
export function formatTagName(tag) {
    return capitalizeFirstLetter(tag).replace(/-|_/g, ' ');
}

/**
 * Takes in a string tag and returns an object with the tag name and colors:
 * {
 *    tagName: String,
 *    tagTextColor: String,
 *    tagBackgroundColor: String,
 * }
 * @async
 * @function formatTag
 * @param {String} tag - The string tag name
 * @returns {Promise<Object>} formattedTags
 */
async function formatTag(tag) {
    const tagName = formatTagName(tag);
    const tagColor = await fetchTagColor(tag);
    
    return {
        tagName: tagName,
        tagTextColor: tagColor.textColor,
        tagBackgroundColor: tagColor.backgroundColor
    };

}

/**
 * Takes in a string tag and makes an api request, returns the tag color object
 * { 
 *      backgroundColor;
        textColor;
 * }
 * @async
 * @function fetchTagColor
 * @param {String} tag
 * @returns {Promise<object>}
 */
async function fetchTagColor(tag) {
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