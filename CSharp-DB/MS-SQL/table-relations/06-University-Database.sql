--06. University Database
CREATE TABLE Majors (
	MajorID INT PRIMARY KEY,
	NAME NVARCHAR(20)
)

CREATE TABLE Students (
	StudentID INT PRIMARY KEY,
	StudentNumber INT,
	StudentName NVARCHAR(20),
	MajorID INT,
	FOREIGN KEY (MajorID)
	REFERENCES Majors(MajorID)
)

CREATE TABLE Payments (
	PaymentID INT PRIMARY KEY,
	PaymentDate NVARCHAR(20),
	PaymentAmount INT,
	StudentID INT 
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID)
)

CREATE TABLE Subjects (
	SubjectID INT PRIMARY KEY,
	SubjectName NVARCHAR(20)
)

CREATE TABLE Agenda (
	StudentID INT,
	SubjectID INT,
	FOREIGN KEY (SubjectID)
	REFERENCES Subjects(SubjectID),
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID)
)