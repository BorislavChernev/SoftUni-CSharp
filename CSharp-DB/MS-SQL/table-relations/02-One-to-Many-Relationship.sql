--02. One-To-Many Relationship
CREATE TABLE Manufacturers (
	ManufacturerID INT PRIMARY KEY NOT NULL,
	Name NVARCHAR(20) NOT NULL,
	EstablishedOn NVARCHAR(20) NOT NULL
)

CREATE TABLE Models (
	ModelID INT PRIMARY KEY,
	Name NVARCHAR(20),
	ManufacturerID INT NOT NULL,
	FOREIGN KEY (ManufacturerID)
	REFERENCES Manufacturers(ManufacturerID)
)