CREATE TABLE [dbo].[CourseInstructors]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [TermCode] INT NOT NULL, 
    [Crn] INT NOT NULL, 
    [InstructorId] INT NULL, 
    [Primary] BIT NOT NULL DEFAULT 0
)
