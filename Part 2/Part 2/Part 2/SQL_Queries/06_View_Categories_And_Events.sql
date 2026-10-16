
-- Part 2 - Inspect the RaceDay database schema

SELECT
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN (
    'RaceDayUsers',
    'Events',
    'Categories',
    'Enrolments',
    'Results',
    'Routes',
    'Weather'
)
ORDER BY TABLE_NAME, ORDINAL_POSITION;
