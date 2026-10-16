
-- Part 2 - Query 3: Count records in each RaceDay table

SELECT 'RaceDayUsers' AS TableName, COUNT(*) AS TotalRecords FROM dbo.RaceDayUsers
UNION ALL
SELECT 'Events', COUNT(*) FROM dbo.Events
UNION ALL
SELECT 'Categories', COUNT(*) FROM dbo.Categories
UNION ALL
SELECT 'Enrolments', COUNT(*) FROM dbo.Enrolments
UNION ALL
SELECT 'Results', COUNT(*) FROM dbo.Results
UNION ALL
SELECT 'Routes', COUNT(*) FROM dbo.Routes
UNION ALL
SELECT 'Weather', COUNT(*) FROM dbo.Weather
ORDER BY TableName;
