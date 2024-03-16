import { capitalizeFirstLetter } from '../../PopNGo/wwwroot/js/util/capitalizeFirstLetter.js';

test('capitalizeFirstLetter first letter of string should be capitalized', () => {
    expect(capitalizeFirstLetter("test word one")).toBe("Test word one");
});
