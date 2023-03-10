CREATE PROC usp_GetTownsStartingWith(@string NVARCHAR(50))
AS
    SELECT Name
    FROM Towns
    WHERE LEFT(Name, LEN(@string)) = @string 
