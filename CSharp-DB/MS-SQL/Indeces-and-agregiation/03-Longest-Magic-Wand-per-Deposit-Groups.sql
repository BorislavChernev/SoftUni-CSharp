--03. Longest Magic Wand per Deposit Groups
SELECT DepositGroup ,MAX(MagicWandSize) AS [LongestMagicWand]
FROM WizzardDeposits AS wd
GROUP BY DepositGroup
