USE Bank

CREATE PROC usp_GetHoldersFullName 
AS
	SELECT FirstName + ' ' + LastName as [Full Name]
	FROM AccountHolders