--04. Smallest Deposit Group Per Magic Wand Size
SELECT TOP(2) DepositGroup
FROM WizzardDeposits AS wd
GROUP BY DepositGroup
HAVING AVG(MagicWandSize) <
(
    SELECT AVG(MagicWandSize) FROM WizzardDeposits
)