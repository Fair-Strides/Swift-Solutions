import { validateBuildEventCardProps } from '../../PopNGo/wwwroot/js/ui/buildEventCard.js';
import { validateBuildEventDetailsModalProps } from '../../PopNGo/wwwroot/js/ui/buildEventDetailsModal.js';


describe('validateBuildEventCardProps', () => {
    test('null props should return false', () => {
        expect(validateBuildEventCardProps(null)).toBe(false);
    });

    test('undefined props should return false', () => {
        expect(validateBuildEventCardProps(undefined)).toBe(false);
    });

    test('non-object props should return false', () => {
        expect(validateBuildEventCardProps(1)).toBe(false);
    });

    test('correct props should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('img not a string should return false', () => {
        const props = {
            img: 1,
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => {}
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('img is null should return true', () => {
        const props = {
            img: null,
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('img is undefined should return true', () => {
        const props = {
            img: undefined,
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('title not a string should return false', () => {
        const props = {
            img: "img",
            title: 1,
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('date not a date should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: 1,
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('city not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: 1,
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('state not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: 1,
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('tags not an array should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: 1,
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('favorited not a boolean should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: 1,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('onPressFavorite not a function should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: 1,
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('onPressFavorite is null should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: null,
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('onPressFavorite is undefined should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: undefined,
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('onPressEvent not a function should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: 1
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('onPressEvent is null should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: null
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('onPressEvent is undefined should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: undefined
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });
});

describe('validateBuildEventDetailsModalProps', () => {
    test('null props should return false', () => {
        expect(validateBuildEventDetailsModalProps(null)).toBe(false);
    });

    test('undefined props should return false', () => {
        expect(validateBuildEventDetailsModalProps(undefined)).toBe(false);
    });

    test('non-object props should return false', () => {
        expect(validateBuildEventDetailsModalProps(1)).toBe(false);
    });

    test('correct props should return true', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('img not a string should return false', () => {
        const props = {
            img: 1,
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('img is null should return true', () => {
        const props = {
            img: null,
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('img is undefined should return true', () => {
        const props = {
            img: undefined,
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('title not a string should return false', () => {
        const props = {
            img: "img",
            title: 1,
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('description not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: 1,
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('date not a date should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: 1,
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('fullAddress not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: 1,
            tags: [],
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('tags not an array should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: 1,
            favorited: false,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('favorited not a boolean should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: 1,
            onPressFavorite: () => { }
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('onPressFavorite not a function should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: 1
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('onPressFavorite is null should return true', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: null
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('onPressFavorite is undefined should return true', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
            favorited: false,
            onPressFavorite: undefined
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });
});