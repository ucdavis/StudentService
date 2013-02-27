CREATE TABLE [dbo].[CourseRoster] (
    [Termcode] INT NOT NULL,
    [Crn]      INT NOT NULL,
    [Pidm]     INT NOT NULL,
    CONSTRAINT [PK_CourseRoster] PRIMARY KEY CLUSTERED ([Termcode] ASC, [Crn] ASC, [Pidm] ASC)
);

