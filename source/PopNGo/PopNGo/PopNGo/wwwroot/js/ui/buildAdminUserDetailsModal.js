import { validateObject } from '../validation.js';
import { updateUserAccount } from '../api/admin/users.js';


let userId = null;
function getUserId() {
    return userId;
}
function setUserId(id) {
    userId = id;
}

/**
 * Takes in the admin's user details modal element and the user info props and updates the user template
 * Props:
 * {
 *   firstName: String,
 *   lastName: String,
 *   userName: String,
 *   email: String,
 *   notificationEmail: String,
 *   weekBefore: Boolean,
 *   dayBefore: Boolean,
 *   dayOf: Boolean,
 * }
 * 
 * @function buildAdminUserDetailsModal
 * @param {HTMLElement} adminUserDetailElement 
 * @param {Object} props 
 */
export const buildAdminUserDetailsModal = (adminDetailsModalElement, props) => {
    setUserId(props.id);

    // Set up modal element variables
    const firstNameElement = adminDetailsModalElement.querySelector('#user-details-first-name');
    const lastNameElement = adminDetailsModalElement.querySelector('#user-details-last-name');
    const userNameElement = adminDetailsModalElement.querySelector('#user-details-username');
    const emailElement = adminDetailsModalElement.querySelector('#user-details-email');
    const notificationEmailElement = adminDetailsModalElement.querySelector('#user-details-notification-email');
    const weekBeforeElement = adminDetailsModalElement.querySelector('#user-details-week-before');
    const dayBeforeElement = adminDetailsModalElement.querySelector('#user-details-day-before');
    const dayOfElement = adminDetailsModalElement.querySelector('#user-details-day-of');
    const saveButtonElement = adminDetailsModalElement.querySelector('#user-details-save');

    // Set the first and last name
    firstNameElement.value = props.firstName;
    lastNameElement.value = props.lastName;

    // Set the username
    userNameElement.value = props.userName;

    // Set the email
    emailElement.value = props.email;
    
    // Set the notification email
    notificationEmailElement.value = props.notificationEmail;

    // Set the notification settings
    weekBeforeElement.checked = props.weekBefore;
    dayBeforeElement.checked = props.dayBefore;
    dayOfElement.checked = props.dayOf;

    // Bind the save button
    saveButtonElement.addEventListener('click', async function() {
        try {
            const data = await updateUserAccount(getUserId(), {
                email: emailElement.value,
                firstName: firstNameElement.value,
                lastName: lastNameElement.value,
                notificationEmail: notificationEmailElement.value,
                weekBefore: weekBeforeElement.checked,
                dayBefore: dayBeforeElement.checked,
                dayOf: dayOfElement.checked,
                userName: userNameElement.value,
                id: props.id,
            });
            console.log(data);
        } catch (error) {
            console.error(error);
        }

        const modal = bootstrap.Modal.getInstance(adminDetailsModalElement);
    });
}


/**
 * Validates the props for the buildAdminUserDetailsModal function, returns true if the props are valid, false otherwise
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildAdminUserDetailsModalProps(data) {
    if (data === undefined || data === null) {
        return false;
    }

    const schema = {
        firstName: x => typeof x === 'string',
        lastName: x => typeof x === 'string',
        userName: x => typeof x === 'string',
        email: x => typeof x === 'string',
        notificationEmail: x => typeof x === 'string',
        weekBefore: x => typeof x === 'boolean',
        dayBefore: x => typeof x === 'boolean',
        dayOf: x => typeof x === 'boolean',
    }

    return validateObject(data, schema).length === 0;
}