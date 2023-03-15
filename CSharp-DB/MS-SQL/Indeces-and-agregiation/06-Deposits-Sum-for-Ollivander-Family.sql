--06. Deposits Sum for Ollivander Family
SELECT DepositGroup, SUM(DepositAmount) AS [TotalSum]
FROM WizzardDeposits AS wd
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup