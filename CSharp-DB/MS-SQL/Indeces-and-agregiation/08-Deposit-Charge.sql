--08. Deposit Charge
SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS [MinDepositCharge]
FROM WizzardDeposits AS wd
GROUP BY DepositGroup, MagicWandCreator
ORDER BY MagicWandCreator, DepositGroup