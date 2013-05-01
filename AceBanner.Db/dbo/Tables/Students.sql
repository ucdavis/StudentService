CREATE TABLE [dbo].[Students] (
    [Pidm]      INT          NOT NULL,
    [StudentId] INT          NULL,
    [FirstName] VARCHAR (50) NOT NULL,
    [LastName]  VARCHAR (50) NOT NULL,
    [LoginId]   VARCHAR (20) NOT NULL,
    [Email]     VARCHAR (50) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Pidm] ASC)
);

