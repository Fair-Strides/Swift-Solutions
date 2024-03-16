import { validateObject } from '../validation.js';

/**
 * Takes in the event details modal element and the event info props and updates the event card element
 * Props:
 * {
 *    img: String link,
 *    title: String,
 *    description: String,
 *    date: DateTime,
 *    fullAddress: String,
 *    tags: Array[Tag],
 *    favorited: Boolean
 *    onPressFavorite: Function
 * }
 * 
 Tag: {
    tagName: String,
    tagTextColor: String,
    tagBackgroundColor: String,
 }

 * @function buildEventDetailsModal
 * @param {HTMLElement} eventDetailElement 
 * @param {Object} props 
 */

export const buildEventDetailsModal = (eventDetailsModalElement, props) => {
    // Set the image
    if (props.img === null || props.img === undefined) {
        eventDetailsModalElement.querySelector('#event-modal-img').src = '/media/images/placeholder_event_card_image.png';
    } else {
        eventDetailsModalElement.querySelector('#event-modal-img').src = props.img;
    }

    // Set the title
    eventDetailsModalElement.querySelector('#event-modal-title').textContent = props.title;

    // Set the datetime
    const time = props.date.toLocaleTimeString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    eventDetailsModalElement.querySelector('#event-modal-date').textContent = props.date.toLocaleString('default', { month: 'long' }) + " " + props.date.getDate() + ", " + props.date.getFullYear() + " at " + time;

    // Set the location
    eventDetailsModalElement.querySelector('#event-modal-location').textContent = props.fullAddress;

    // Set the favorite status
    const bookmarkContainer = eventDetailsModalElement.querySelector('#event-modal-bookmark-container');
    const bookmarkImage = eventDetailsModalElement.querySelector('#event-modal-bookmark-icon');
    bookmarkImage.src = props.favorited ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';

    bookmarkContainer.addEventListener('click', () => {
        props.onPressFavorite();
        props.favorited = !props.favorited;
        bookmarkImage.src = props.favorited ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';
    });

    // Set the tags
    const tagsContainer = eventDetailsModalElement.querySelector('#modal-event-card-tags-container');
    tagsContainer.innerHTML = '';

    for (let tag of props.tags) {
        const tagElement = document.createElement('span');
        tagElement.classList.add('tag');
        tagElement.textContent = tag.tagName;
        tagElement.style.color = tag.tagTextColor;
        tagElement.style.backgroundColor = tag.tagBackgroundColor;
        tagsContainer.appendChild(tagElement);
    }

    // Set the description
    eventDetailsModalElement.querySelector('#modal-event-description').textContent = props.description;
}


/**
 * Validates the props for the buildEventDetailsModal function, returns true if the props are valid, false otherwise
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildEventDetailsModalProps(data) {
    if (data === undefined || data === null) {
        return false;
    }

    const schema = {
        img: x => (typeof x === 'string' || x === undefined || x === null),
        title: x => typeof x === 'string',
        description: x => typeof x === 'string',
        date: x => x instanceof Date,
        fullAddress: x => typeof x === 'string',
        tags: x => Array.isArray(x),
        favorited: x => typeof x === 'boolean',
        onPressFavorite: x => (typeof x === 'function' || x === undefined || x === null),
    }

    return validateObject(data, schema).length === 0;
}