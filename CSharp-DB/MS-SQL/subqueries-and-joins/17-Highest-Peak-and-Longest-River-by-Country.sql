--17. Highest Peak and Longest River by Country
SELECT TOP (5) CountryName,
MAX(p.Elevation) AS [HighestPeakElevation],
MAX(r.Length) AS [HighestPeakElevation]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
LEFT JOIN Peaks AS p ON mc.MountainId = p.MountainId
LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
GROUP BY c.CountryName
ORDER BY MAX(p.Elevation) DESC, MAX(r.Length) DESC, c.CountryName