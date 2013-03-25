CREATE TABLE [dbo].[Instructors]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [LoginId] VARCHAR(20) NULL, 
    [Email] VARCHAR(50) NULL
)
