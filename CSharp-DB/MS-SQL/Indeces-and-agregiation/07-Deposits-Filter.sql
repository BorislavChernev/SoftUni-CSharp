--07. Deposits Filter
SELECT DepositGroup, SUM(DepositAmount) AS [TotalSum]
FROM WizzardDeposits AS wd
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY SUM(DepositAmount) DESC