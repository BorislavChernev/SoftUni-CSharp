--16. Get Users with IP Address Like Pattern
SELECT Username, IpAddress
FROM Users
WHERE IpAddress LIKE('___.1_%._%.___')
ORDER BY Username