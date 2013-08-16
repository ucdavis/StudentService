CREATE TABLE [dbo].[Sections] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [TermCode]    INT              NOT NULL,
    [Crn]         INT              NOT NULL,
    [SectionType] CHAR (3)         NOT NULL,
    [StartDate]   DATE             NOT NULL,
    [EndDate]     DATE             NOT NULL,
    [StartTime]   TIME (0)         NULL,
    [EndTime]     TIME (0)         NULL,
    [DaysOfWeek]  VARCHAR (5)      NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [Sections_TermCode_CVIDX]
    ON [dbo].[Sections]([TermCode] ASC)
    INCLUDE([Crn], [SectionType], [StartDate], [EndDate], [StartTime], [EndTime], [DaysOfWeek]);

