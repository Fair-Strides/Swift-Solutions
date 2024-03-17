import { formatTagName } from '../../PopNGo/wwwroot/js/util/tags.js';

describe('formatTagName', () => {
    test('should replace underscores with spaces', () => {
        expect(formatTagName("Test_tag")).toBe("Test tag");
    });

    test('should capitalize first letter', () => {
        expect(formatTagName("testtag")).toBe("Testtag");
    });

    test('should make tag readable', () => {
        expect(formatTagName("test_tag")).toBe("Test tag");
    });
});
