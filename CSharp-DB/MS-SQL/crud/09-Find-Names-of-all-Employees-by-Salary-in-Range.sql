--09. Find Names of All Employees by Salary in Range
SELECT FirstName, LastName, JobTitle
	FROM Employees
	WHERE
	Salary >= 20000
	AND 
	Salary <= 30000