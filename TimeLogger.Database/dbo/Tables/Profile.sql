CREATE TABLE [dbo].[Profile] (
    [EmpId]       INT                IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)      NOT NULL,
    [DateOfBirth] DATETIMEOFFSET (7) NULL,
    [Designation] NVARCHAR (50)      NULL,
    [Experience]  FLOAT (53)         NULL,
    [Token]       VARCHAR (MAX)      NULL,
    [UserId]      INT                NULL,
    [Password]    NVARCHAR (50)      NOT NULL,
    CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED ([EmpId] ASC),
    CONSTRAINT [FK_Profile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

