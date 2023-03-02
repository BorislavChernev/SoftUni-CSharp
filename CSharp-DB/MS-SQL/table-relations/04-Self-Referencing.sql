--04. Self-Referencing
CREATE TABLE Teachers (
	TeacherID INT PRIMARY KEY,
	Name NVARCHAR(20),
	ManagerID INT
	FOREIGN KEY (TeacherID)
	REFERENCES Teachers(TeacherID)
)