
-- Part 2 - Query 6: View categories and their events

SELECT
    c.*,
    e.*
FROM dbo.Categories AS c
INNER JOIN dbo.Events AS e
    ON c.EventID = e.EventID
ORDER BY e.EventID;
