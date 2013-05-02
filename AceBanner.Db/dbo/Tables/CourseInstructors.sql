CREATE TABLE [dbo].[CourseInstructors]
(
    [TermCode] INT NOT NULL, 
    [Crn] INT NOT NULL, 
    [LoginId] VARCHAR(20) NOT NULL, 
    [Primary] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_CourseInstructors] PRIMARY KEY ([TermCode], [Crn], [LoginId])
)
