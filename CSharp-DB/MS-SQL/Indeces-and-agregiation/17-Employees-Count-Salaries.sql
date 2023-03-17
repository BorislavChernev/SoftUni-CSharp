--17. Employees Count Salaries
SELECT COUNT(*) AS Count
FROM Employees 
WHERE ManagerID IS NULL
GROUP BY ManagerID
