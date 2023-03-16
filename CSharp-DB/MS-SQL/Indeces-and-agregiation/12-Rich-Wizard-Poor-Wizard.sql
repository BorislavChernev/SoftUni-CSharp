--12. *Rich Wizard, Poor Wizard
SELECT SUM(SMTH.DIFF)
FROM (SELECT DepositAmount - (SELECT DepositAmount FROM WizzardDeposits WHERE Id = n.Id +1)
AS DIFF FROM WizzardDeposits n) AS SMTH
