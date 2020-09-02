CREATE TABLE [dbo].[User] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (20) NOT NULL,
    [Password] NVARCHAR (20) NOT NULL,
    [Email]    NVARCHAR (50) NOT NULL,
    [Phone]    NVARCHAR (15) NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_User]
    ON [dbo].[User]([Email] ASC);

