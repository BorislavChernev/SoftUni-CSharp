CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(50), @word NVARCHAR(50))
RETURNS BIT
AS
BEGIN
    DECLARE @result BIT = 1
    DECLARE @loopEnd INT = LEN(@word)
    DECLARE @currentLetter CHAR
    DECLARE @i INT = 1;

    WHILE (@i <= @loopEnd)
    BEGIN
            SET @currentLetter = SUBSTRING(@word, @i, 1)

            IF(CHARINDEX(@currentLetter, @setOfLetters) > 0)
            BEGIN
                    SET @i += 1;
                    CONTINUE
            END
            ELSE
            BEGIN
                    SET @result = 0;
                    BREAK
            END
    END

    RETURN @result

END