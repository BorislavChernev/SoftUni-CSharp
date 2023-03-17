--15. Employees Average Salaries
SELECT * INTO [Rich] FROM Employees
WHERE [Salary] > 30000

DELETE FROM Rich
WHERE [ManagerID] = 42
 
UPDATE Rich
SET [Salary] += 5000
WHERE [DepartmentID] = 1
 
SELECT [DepartmentID],
    AVG([Salary]) as [AverageSalary]
FROM Rich
GROUP BY [DepartmentID]