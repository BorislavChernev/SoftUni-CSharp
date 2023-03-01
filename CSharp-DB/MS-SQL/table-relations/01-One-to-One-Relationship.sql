--01. One-To-One Relationship
CREATE TABLE Passports (
	PassportID INT PRIMARY KEY,
	PassportNumber NVARCHAR(100),
)

CREATE TABLE Persons (
	PersonID INT PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(100),
	Salary INT,
	PassportID INT UNIQUE NOT NULL,
	FOREIGN KEY (PassportID)
	REFERENCES Passports(PassportID)
)