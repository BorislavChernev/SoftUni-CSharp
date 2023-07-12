CREATE DATABASE CigarShop
 
USE CigarShop

-- Problem 1

CREATE TABLE Sizes(
Id INT IDENTITY PRIMARY KEY,
Length INT NOT NULL
  CHECK (Length>=10 AND Length<=25),
RingRange DECIMAL(18,2)
 CHECK (RingRange>=1.5 AND RingRange<=7.5)
)

CREATE TABLE Tastes(
Id INT IDENTITY PRIMARY KEY,
TasteType VARCHAR(20) NOT NULL,
TasteStrength VARCHAR(15) NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Brands(
Id INT IDENTITY PRIMARY KEY,
BrandName  VARCHAR(30) UNIQUE NOT NULL,
BrandDescription VARCHAR(max) 
)

CREATE TABLE Cigars(
Id INT IDENTITY PRIMARY KEY,
CigarName VARCHAR(80) NOT NULL,
BrandId INT FOREIGN KEY REFERENCES Brands(Id) NOT NULL,
TastId INT FOREIGN KEY REFERENCES Tastes(Id) NOT NULL,
SizeId INT FOREIGN KEY REFERENCES Sizes(Id) NOT NULL,
PriceForSingleCigar MONEY NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Addresses(
Id INT IDENTITY PRIMARY KEY,
Town VARCHAR(30) NOT NULL,
Country NVARCHAR(30)  NOT NULL,
Streat NVARCHAR(100) NOT NULL,
ZIP VARCHAR(20) NOT NULL
)

CREATE TABLE Clients(
Id INT IDENTITY PRIMARY KEY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Email NVARCHAR(30) NOT NULL,
AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL,
)

CREATE TABLE ClientsCigars(
ClientId INT FOREIGN KEY REFERENCES Clients(Id),
CigarId INT FOREIGN KEY REFERENCES Cigars(Id)
PRIMARY KEY(CigarId,ClientId)
)

-- PROBLEM 2

INSERT INTO Cigars(CigarName, BrandId, TastId, SizeId, PriceForSingleCigar, ImageURL)
 VALUES
 ('COHIBA ROBUSTO', 9,1 ,5 ,15.50, 'cohiba-robusto-stick_18.jpg'),
 ('COHIBA SIGLO I',9 , 1,10 ,410.00,'cohiba-siglo-i-stick_12.jpg'),
 ('HOYO DE MONTERREY LE HOYO DU MAIRE', 14, 5,11 ,7.50,'hoyo-du-maire-stick_17.jpg'),
 ('HOYO DE MONTERREY LE HOYO DE SAN JUAN',14 , 4, 15,32.00,'hoyo-de-san-juan-stick_20.jpg'),
 ('TRINIDAD COLONIALES', 2, 3, 8,85.21,' trinidad-coloniales-stick_30.jpg')

 INSERT INTO Addresses(Town, Country, Streat, ZIP) 
   VALUES
   ('Sofia','Bulgaria','18 Bul. Vasil levski',1000),
      ('Athens','Greece','4342 McDonald Avenue',10435),
	     ('Zagreb','Croatia','4333 Lauren Drive',10000)

-- PROBLEM 3

UPDATE Cigars
SET PriceForSingleCigar += PriceForSingleCigar * 0.2
Where TastId = (
SELECT id FROM Tastes
WHERE TasteType = 'Spicy')

UPDATE Brands
SET BrandDescription = 'New description'
WHERE BrandDescription IS NULL

-- PROBLEM 4

DELETE FROM Clients
WHERE AddressId IN (
SELECT Id
FROM Addresses
WHERE Country LIKE 'C%'
)

DELETE FROM Addresses
WHERE Country LIKE 'C%'

-- PROBLEM 5

SELECT CigarName, PriceForSingleCigar, ImageURL
FROM Cigars
ORDER BY PriceForSingleCigar ASC, CigarName DESC

-- PROBLEM 6

SELECT c.Id, c.CigarName, c.PriceForSingleCigar, t.TasteType, t.TasteStrength
FROM [Cigars] AS [c]
JOIN [Tastes] AS [t] ON c.TastId = t.Id
WHERE t.TasteType IN ('Earthy', 'Woody')
ORDER BY PriceForSingleCigar DESC

-- PROBLEM 7

SELECT c.id, CONCAT(c.FirstName, ' ', c.LastName) AS [ClientName], c.Email
FROM [Clients] AS [c]
LEFT JOIN [ClientsCigars] AS [cc] ON c.Id = cc.ClientId
WHERE cc.ClientId IS NULL
ORDER BY ClientName ASC

-- PROBELM 8

SELECT TOP(5)
c.CigarName, c.PriceForSingleCigar, c.ImageURL
FROM [Cigars] AS [c]
JOIN [Sizes] AS [s]
ON c.SizeId = s.Id
WHERE s.Length >= 12
AND (c.CigarName LIKE '%ci%'
OR c.PriceForSingleCigar >= 50)
AND s.RingRange > 2.55
ORDER BY c.CigarName ASC, c.PriceForSingleCigar DESC

-- PROBLEM 9

SELECT
CONCAT(c.FirstName, ' ', c.LastName) AS [FullName],
a.Country,
a.ZIP,
MAX(FORMAT(cr.PriceForSingleCigar, 'c')) AS [CigarPrice]
FROM [Clients] AS [c]
JOIN [Addresses] AS [a] 
ON c.AddressId = a.Id
JOIN [ClientsCigars] AS [cc]
ON c.Id = cc.ClientId
JOIN [Cigars] AS [cr]
ON cr.Id = cc.CigarId
WHERE a.ZIP NOT LIKE '%[^0-9]%'
GROUP BY CONCAT(c.FirstName, ' ', c.LastName), a.Country, a.ZIP
ORDER BY FullName ASC


--'%[^0-9]%'
--FORMAT(cr.PriceForSingleCigar, 'c') AS [CigarPrice] 
SELECT * FROM Sizes
SELECT * FROM ClientsCigars
SELECT * FROM Cigars
SELECT * FROM Addresses
SELECT * FROM Clients
