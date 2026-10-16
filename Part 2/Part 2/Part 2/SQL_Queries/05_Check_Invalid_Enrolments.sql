
-- Inspect the actual columns in the Enrolments table
SELECT
    COLUMN_NAME,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Enrolments'
ORDER BY ORDINAL_POSITION;

