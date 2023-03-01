--24. Countries and Currency (Euro / Not Euro)
SELECT
	CountryName,
	CountryCode,
	IF('CurrencyCpde' = 'EUR', 'Euro', 'Not Euro') AS Currency
FROM Countries
ORDER BY CountryName