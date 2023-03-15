--05. Deposits Sum
SELECT DepositGroup, SUM(DepositAmount) AS [TotalSum]
FROM WizzardDeposits AS wd
GROUP BY DepositGroup