
import { formatStartTime, formatDateWithWeekday, formatHourMinute } from '../../PopNGo/wwwroot/js/util/formatStartTime.js';

test('Testing time from Real-Time Event Search API', () => {
    expect(formatStartTime("2024-02-15T21:45:00")).toBe("2/15/2024, 09:45 PM");
});

test('Testing time from Database', () => {
    expect(formatStartTime("2024-03-02 11:00:00.000")).toBe("3/2/2024, 11:00 AM");
});

test('Testing time from Real-Time Event Search API', () => {
    expect(formatDateWithWeekday("2024-02-15T21:45:00")).toBe("Thursday, February 15, 2024");
});

test('Testing time from Database', () => {
    expect(formatDateWithWeekday("2024-03-02 11:00:00.000")).toBe("Saturday, March 2, 2024");
});

test('Testing time from Real-Time Event Search API', () => {
    expect(formatHourMinute("2024-02-15T21:45:00")).toBe("9:45 PM");
});

test('Testing time from Database', () => {
    expect(formatHourMinute("2024-03-02 11:00:00.000")).toBe("11:00 AM");
});
