
// Expects a schema like this of what properties the object has
const schemaExample = {
    magnitude: x => typeof x === 'number' && x >= 0,                // check type and value
    location: x => typeof x === 'string' && /[A-Za-z0-9]*/.test(x), // can test by regex
    eTime: x => typeof x === 'number'
};

/**
 * Pass in the object to validate properties on, and the schema that defines what properties should exist and a validation function to run.
 * Returns an array that is empty(passes validation), or contains Error object corresponding to property names that failed validation
 * @param {any} obj
 * @param {any} schema
 * @returns {Error[]}
 */
export const validateObject = (obj, schema) => Object
    .keys(schema)
    .filter(key => !schema[key](obj[key]))
    .map(key => new Error(`${key}`));

/**
 * Pass in an array of objects and a schema to validate each object against.
 * @param {any[]} arr
 * @param {any} schema
 * @returns
 */
export const validateArrayOfObjects = (arr, schema) => arr
    .map(obj => validateObject(obj, schema))
    .reduce((acc, arr) => arr.length > 0 ? acc + 1 : 0, 0);  // use a fold to count up the number of entries whose length is > 0