CREATE PROCEDURE usp_EmployeesBySalaryLevel(@level NVARCHAR(50))
AS
    SELECT FirstName, LastName
    FROM Employees
    WHERE dbo.ufn_GetSalaryLevel(Salary) = @level