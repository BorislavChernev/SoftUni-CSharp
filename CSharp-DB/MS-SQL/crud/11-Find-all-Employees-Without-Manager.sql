--11. Find All Employees Without Manager
SELECT FirstName, LastName
	FROM Employees
	WHERE ManagerID IS NULL;