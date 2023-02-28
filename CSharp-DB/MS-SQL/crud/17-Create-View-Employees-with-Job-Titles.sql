--17. Create View Employees with Job Titles

CREATE VIEW [V_EmployeesSalaries] AS
	SELECT
		CONCAT([FirstName],
			' ',
			COALESCE(MiddleName, ''),
			' ',
			LastName) AS [Full Name], JobTitle
	FROM
	Employees 