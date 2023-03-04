--08. Employee 24
SELECT e.EmployeeID, e.FirstName, IIF(YEAR(p.StartDate) >= 2005, NULL, p.Name) AS ProjectName
FROM Employees AS e
JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
JOIN Projects AS p ON ep.ProjectID = p.ProjectID
WHERE e.EmployeeID = 24 