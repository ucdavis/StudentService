CREATE TABLE [dbo].[Instructors]
(
	[LoginId] VARCHAR(20) NOT NULL PRIMARY KEY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [Email] VARCHAR(50) NULL, 
    [Pidm] INT NULL
)
