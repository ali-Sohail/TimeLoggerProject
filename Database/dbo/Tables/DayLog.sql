CREATE TABLE [dbo].[DayLog] (
    [Id]          INT                IDENTITY (1, 1) NOT NULL,
    [EmpId]       INT                NOT NULL,
    [InTime]      DATETIMEOFFSET (7) NOT NULL,
    [OutTime]     DATETIMEOFFSET (7) NULL,
    [Description] NVARCHAR (50)      NULL,
    CONSTRAINT [PK_DayLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DayLog_Profile] FOREIGN KEY ([EmpId]) REFERENCES [dbo].[Profile] ([EmpId])
);

