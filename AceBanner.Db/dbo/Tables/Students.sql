CREATE TABLE [dbo].[Students] (
    [LoginId]   VARCHAR (20) NOT NULL,
	[FirstName] VARCHAR (50) NOT NULL,
    [LastName]  VARCHAR (50) NOT NULL,
	[Pidm]      INT          NULL,
    [StudentId] INT          NULL,
    [Email]     VARCHAR (50) NULL,
    [Type] CHAR NOT NULL, 
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([LoginId])
);

