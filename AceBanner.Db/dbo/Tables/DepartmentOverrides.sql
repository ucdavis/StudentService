CREATE TABLE [dbo].[DepartmentOverrides]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Subject] VARCHAR(5) NOT NULL, 
    [CourseNumb] VARCHAR(5) NULL, 
    [DepartmentId] VARCHAR(4) NOT NULL
)
