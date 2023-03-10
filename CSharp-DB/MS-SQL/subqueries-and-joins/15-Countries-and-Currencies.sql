--15. *Continents and Currencies
SELECT rc.ContinentCode, rc.CurrencyCode, rc.[Count] AS CurrencyUsage FROM(
	SELECT c.ContinentCode, c.CurrencyCode, COUNT(c.CurrencyCode) AS [Count], DENSE_RANK() OVER (PARTITION BY c.ContinentCode ORDER BY COUNT(c.CurrencyCode) DESC) AS [rank] 
	FROM Countries AS c
	GROUP BY c.ContinentCode, c.CurrencyCode) AS rc
WHERE rc.rank = 1 AND rc.[Count] > 1 