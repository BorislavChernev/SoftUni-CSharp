--13. Find 5 Best Paid Employees
SELECT TOP(5) FirstName, LastName
	FROM Employees
	ORDER BY Salary DESC