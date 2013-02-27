CREATE TABLE [dbo].[Sections] (
	[Id] uniqueidentifier not null,
    [TermCode]    INT         NOT NULL,
    [Crn]         INT         NOT NULL,
    [SectionType] CHAR (3)    NOT NULL,
    [StartDate]   DATE        NOT NULL,
    [EndDate]     DATE        NOT NULL,
    [StartTime]   TIME (0)    NULL,
    [EndTime]     TIME (0)    NULL,
    [DaysOfWeek]  VARCHAR (5) NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED ([Id])
);

