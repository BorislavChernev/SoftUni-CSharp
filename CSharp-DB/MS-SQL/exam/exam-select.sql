--01. DDL
CREATE TABLE Owners(
Id INT IDENTITY PRIMARY KEY,
Name VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(15) NOT NULL,
Address VARCHAR(50)
)

CREATE TABLE AnimalTypes(
Id INT IDENTITY PRIMARY KEY,
AnimalType VARCHAR(30) NOT NULL
)

CREATE TABLE Cages(
Id INT IDENTITY PRIMARY KEY,
AnimalTypeId INT NOT NULL FOREIGN KEY REFERENCES AnimalTypes(Id)
)

CREATE TABLE Animals(
Id INT IDENTITY PRIMARY KEY,
Name VARCHAR(30) NOT NULL,
BirthDate DATE NOT NULL,
OwnerId INT FOREIGN KEY REFERENCES Owners(Id),
AnimalTypeId INT NOT NULL FOREIGN KEY REFERENCES AnimalTypes(Id)
)

CREATE TABLE AnimalsCages(
CageId INT NOT NULL FOREIGN KEY REFERENCES Cages(Id),
AnimalId INT NOT NULL FOREIGN KEY REFERENCES Animals(Id),
PRIMARY KEY (CageId, AnimalId)
)

CREATE TABLE VolunteersDepartments(
Id INT IDENTITY PRIMARY KEY,
DepartmentName VARCHAR(30) NOT NULL
)

CREATE TABLE Volunteers(
Id INT IDENTITY PRIMARY KEY,
Name VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(15) NOT NULL,
Address VARCHAR(50),
AnimalId INT FOREIGN KEY REFERENCES Animals(Id),
DepartmentId INT NOT NULL FOREIGN KEY REFERENCES VolunteersDepartments(Id)
)

--05. Volunteers
SELECT Name, PhoneNumber, Address, AnimalId, DepartmentId
FROM Volunteers
ORDER BY Name, AnimalId, DepartmentId 

--06. Animals data
SELECT a.Name, at.AnimalType, FORMAT(a.BirthDate, 'dd.MM.yyyy') AS BirthDate
FROM Animals AS a
JOIN AnimalTypes AS at ON a.AnimalTypeId = at.Id
ORDER BY Name

--07. Owners and Their Animals
SELECT TOP(5) o.Name, COUNT(a.OwnerId) AS [CountOfAnimals]
FROM Owners AS o
JOIN Animals AS a ON o.Id = a.OwnerId
GROUP BY o.Name
ORDER BY CountOfAnimals DESC, [Name]

--08. Owners, Animals and Cages
SELECT CONCAT(o.Name, '-', a.Name) AS [OwnersAnimals], o.PhoneNumber, ac.CageId
FROM Owners AS o
JOIN Animals AS a ON o.Id = a.OwnerId
JOIN AnimalsCages AS ac ON a.Id = ac.AnimalId
WHERE a.AnimalTypeId = 1
ORDER BY o.Name, a.Name DESC

--09. Volunteers in Sofia
SELECT v.Name, v.PhoneNumber, SUBSTRING(Address, CHARINDEX(',', Address) + 2, LEN(v.Address)) AS Address
FROM Volunteers AS v
WHERE v.Address LIKE ('%Sofia%') AND v.DepartmentId = 2
ORDER BY v.Name

--10. Animals for Adoption
SELECT a.Name, YEAR(a.BirthDate) AS [BirthYear], at.AnimalType
FROM Animals AS a
JOIN AnimalTypes AS at ON a.AnimalTypeId = at.Id
WHERE
    a.OwnerId IS NULL
    AND a.AnimalTypeId != 3
    AND DATEDIFF(YEAR, BirthDate, '01/01/2022') < 5
ORDER BY a.Name

--11. All Volunteers in a Department
GO
CREATE FUNCTION udf_GetVolunteersCountFromADepartment(@VolunteersDepartment VARCHAR (30))
RETURNS INT
AS
BEGIN	
    RETURN(
        SELECT COUNT(*)
        FROM Volunteers AS v
        JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
        WHERE vd.DepartmentName = @VolunteersDepartment
    )
END
GO

SELECT dbo.udf_GetVolunteersCountFromADepartment ('Education program assistant')

--12. Animals with Owner or Not
GO
CREATE PROCEDURE usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(30))
AS
BEGIN
    IF(SELECT OwnerId FROM Animals
    Where Name = @AnimalName) IS NULL
    BEGIN
        SELECT Name, 'For adoption' AS OwnerName
        FROM Animals
        WHERE Name = @AnimalName
    END
    ELSE
    BEGIN
        SELECT a.Name, o.Name as OwnerName
        FROM Animals AS a
        JOIN Owners As o ON o.Id = a.OwnerId
        WHERE a.Name = @AnimalName
    END
END
EXEC usp_AnimalsWithOwnersOrNot 'Pumpkinseed Sunfish'
GO

SELECT * FROM Animals
SELECT * FROM AnimalTypes
SELECT * FROM Owners
SELECT * FROM AnimalsCages
SELECT * FROM VolunteersDepartments
SELECT * FROM Volunteers