CREATE PROC usp_GetHoldersWithBalanceHigherThan (@number MONEY)
AS
	SELECT FirstName, LastName 
	FROM AccountHolders AS ah
	JOIN Accounts AS ac ON ac.AccountHolderId = ah.Id
	GROUP BY FirstName, LastName
	HAVING SUM(Balance) > @number