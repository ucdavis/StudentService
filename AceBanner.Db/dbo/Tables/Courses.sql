CREATE TABLE [dbo].[Courses] (
    [TermCode]               INT          NOT NULL,
    [Crn]                    INT          NOT NULL,
    [Subject]                VARCHAR (5)  NOT NULL,
    [CourseNumb]             VARCHAR (5)  NOT NULL,
    [Sequence]               VARCHAR (5)  NULL,
    [Name]                   VARCHAR (50) NOT NULL,
    [DepartmentId]           VARCHAR (4)  NOT NULL,
    [GradesOutstandingCount] INT          NOT NULL,
    [Gradable]               CHAR (1)     NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([TermCode] ASC, [Crn] ASC),
    CONSTRAINT [FK_Courses_Courses] FOREIGN KEY ([TermCode], [Crn]) REFERENCES [dbo].[Courses] ([TermCode], [Crn])
);








GO
CREATE NONCLUSTERED INDEX [Courses_TermCodeSubjectCourseNumb_IDX]
    ON [dbo].[Courses]([TermCode] ASC, [Subject] ASC, [CourseNumb] ASC);


GO
CREATE NONCLUSTERED INDEX [Courses_TermcodeCoursenumb_IDX]
    ON [dbo].[Courses]([TermCode] ASC, [CourseNumb] ASC);

