CREATE TABLE [dbo].[UserAuthorisation]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ULN] BIGINT NOT NULL,
    [AuthorisedAt] DATETIME2 NOT NULL
)
GO

CREATE UNIQUE INDEX UX_UserAuthorisation_ULN ON dbo.UserAuthorisation (ULN);
GO

CREATE UNIQUE INDEX UX_UserAuthorisation_UserId ON dbo.UserAuthorisation (UserId);
GO

ALTER TABLE [dbo].[UserAuthorisation] ADD CONSTRAINT FK_UserAuthorisation_User
FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]);
GO

