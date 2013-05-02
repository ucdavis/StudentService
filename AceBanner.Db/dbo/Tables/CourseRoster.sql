CREATE TABLE [dbo].[CourseRoster] (
    [Termcode] INT NOT NULL,
    [Crn]      INT NOT NULL,
    [LoginId]     VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_CourseRoster] PRIMARY KEY CLUSTERED ([Termcode] ASC, [Crn] ASC, [LoginId] ASC)
);

