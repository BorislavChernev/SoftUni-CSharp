--07.Find Towns Not Starting With
SELECT TownID, Name
FROM Towns
WHERE Name NOT LIKE 'R%' AND Name NOT LIKE 'B%' AND Name NOT LIKE 'D%'
ORDER BY Name