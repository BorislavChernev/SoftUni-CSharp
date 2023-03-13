CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS NVARCHAR(10)
AS
BEGIN
    DECLARE @level NVARCHAR(10)

    IF(@salary < 30000)
        SET @level = 'Low'
    ELSE IF(@salary > 50000)
        SET @level = 'High'
    ELSE
    SET @level = 'Average'
    RETURN @level
END