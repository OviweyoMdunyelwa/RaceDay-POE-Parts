
-- Part 2 - Query 4: Check for duplicate user emails

SELECT
    Email,
    COUNT(*) AS Occurrences
FROM dbo.RaceDayUsers
GROUP BY Email
HAVING COUNT(*) > 1;
