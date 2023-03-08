--13. Mix of Peak and River Names
SELECT PeakName,
       RiverName,
       LOWER(CONCAT(LEFT(PeakName, LEN(PeakName)-1), RiverName)) AS Mix
FROM Peaks
     JOIN Rivers ON RIGHT(PeakName, 1) = LEFT(RiverName, 1)
ORDER BY Mix;