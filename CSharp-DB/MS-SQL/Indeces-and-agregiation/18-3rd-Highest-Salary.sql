--18. *3rd Highest Salary
SELECT s.DepartmentID,
       s.Salary
FROM
(
    SELECT DepartmentID,
           Salary, 
           DENSE_RANK() OVER(PARTITION BY DepartmentID ORDER BY Salary DESC) AS Rank
    FROM Employees
    GROUP BY DepartmentID,
             Salary
) AS s
WHERE Rank = 3
GROUP BY s.DepartmentID,
         s.Salary;